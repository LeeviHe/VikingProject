using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IWeaponParent {
    
    [Header("Behaviours")]
    public PlayerMovement playerMovement;
    public PlayerAnimations playerAnimations;
    public PlayerCombat playerCombat;
    public PlayerInteract playerInteract;
    [Header("Equipped weapon fields")]
    public Weapon weapon;
    public Transform weaponHoldingPoint;
    public BlessingSO currentBlessing;
    // Spawn point of ability 
    // CHANGE!!
    public Transform abilityOrigin;
    [Header("Health Sliders")]
    public float currentHealth; //Handle player's health
    public float currentMoveSpeed;

    [Header("States")]
    public bool isAlive = true;
    public bool isBlocking = false;
    public bool isFightMode = false;

    [Header("Quests")]
    public QuestSO currentQuest; // Active quest for player
    public List<QuestSO> openQuests; //List of all quest set for player

    public PlayerStatsSO stats; //Player stats
    private float nextAttackTime = 0f; //Time when next attack is allowed
    private float attackDefaultCooldown = 2f;

    //public static event Action OnPlayerWin;
    public static event Action OnQuestActivated;
    public static event Action OnReadyToLeave;

    private void Start() {
        isAlive = true;
        UpdatePlayerObject();
        if (weapon) {
            Weapon.SpawnWeapon(weapon.weaponSO, this, weapon.weaponSO.prefab.transform.rotation);
        }
    }

    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;
    private Vector2 lookInput;
    [SerializeField] private float movementSmoothingSpeed;

    private void Update() {
        if (isAlive) {
            CalculateMovementInputSmoothing();
            UpdatePlayerMovement();
            UpdatePlayerAnimationMovement();
        } else {
            playerAnimations.enabled = false;
        }
    }
    private void ActivateSpecialAbility() {
        if (currentBlessing != null && currentBlessing.specialAbilities.Length > 0 && isFightMode) {
            // Activate the special ability associated with the current blessing
            currentBlessing.specialAbilities[0].ActivateAbility(abilityOrigin);
        }
    }
    public void OnInteract( InputAction.CallbackContext value ) {
        if (value.started) {
            playerInteract.HandleInteract();
        }
    }

    public void OnMove( InputAction.CallbackContext value ) {
        // Get normalized input vector
        Vector2 inputVector = value.ReadValue<Vector2>();

        // Apply skewing transformation for isometric movement
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedInput = matrix.MultiplyPoint3x4(new Vector3(inputVector.x, 0f, inputVector.y));

        // Calculate movement direction in world space
        rawInputMovement = new Vector3(skewedInput.x, 0f, skewedInput.z).normalized;
    }

    public void OnAttack( InputAction.CallbackContext value ) {
        if (value.started && weapon != null && playerCombat.attackDuration <= 0 && isFightMode) {
            playerAnimations.PlayAttackAnimation();
            nextAttackTime = Time.time + attackDefaultCooldown / weapon.attackSpeed;
            playerCombat.attackDuration = (nextAttackTime - Time.time);
            StartCoroutine(playerCombat.PerformAttack());
        }
    }

    public void OnSpell1( InputAction.CallbackContext value ) {
        if (value.started) {
            ActivateSpecialAbility();
        }
    }

    public void OnBlock( InputAction.CallbackContext value ) {
        if (weapon != null && isFightMode) {
            if (value.started) {
                playerAnimations.PlayBlockAnimation(true);
                isBlocking = true;
            } else if (value.canceled) {
                playerAnimations.PlayBlockAnimation(false);
                isBlocking = false;
            }
        }
    }
    public void OnLook( InputAction.CallbackContext value ) {
        lookInput = value.ReadValue<Vector2>();
    }

    public void OnToggleFightMode( InputAction.CallbackContext value ) {
        if (value.started) {
            isFightMode = !isFightMode;

            if (isFightMode) {
                EnterFightMode();
            } else {
                ExitFightMode();
            }
        }
        
    }
    private void EnterFightMode() {
        // Perform actions to enter fight mode
        Debug.Log("Entered Fight Mode");
        playerAnimations.SwitchAttackModeAnimation();
        currentMoveSpeed *= 0.5f;
    }

    // Method to handle exiting fight mode
    private void ExitFightMode() {
        // Perform actions to exit fight mode
        Debug.Log("Exited Fight Mode");
        playerAnimations.SwitchAttackModeAnimation();
        currentMoveSpeed /= 0.5f;
    }
    public void EquipBlessing( BlessingSO blessingSO ) {
        // Remove effects of previously equipped blessing (if any)
        RemoveBlessingEffects();
        Debug.Log(blessingSO);
        currentBlessing = blessingSO;
        // Apply new blessing effects
        blessingSO.ApplyBlessing(this);
        PlayerData.Instance.UpdateBlessing(blessingSO);
    }

    public void ModifyMovementSpeed( float modifier ) {
        float newActiveMoveSpeed = stats.movementSpeed;
        newActiveMoveSpeed += modifier;
        PlayerData.Instance.UpdateSpeed(newActiveMoveSpeed);
        currentMoveSpeed = newActiveMoveSpeed;
    }

    public void ModifyHealth( int modifier ) {
        float newActiveMaxHealth = stats.maxHealth;
        newActiveMaxHealth += modifier;
        PlayerData.Instance.UpdateHealth(newActiveMaxHealth);
        currentHealth = newActiveMaxHealth;
    }

    private void RemoveBlessingEffects() {
        // Reset stats to base values
        currentMoveSpeed = stats.movementSpeed;
        currentHealth = stats.maxHealth;
    }
    void CalculateMovementInputSmoothing() {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

    void UpdatePlayerMovement() {
        playerMovement.UpdateMovementData(smoothInputMovement, lookInput);
    }

    void UpdatePlayerAnimationMovement() {
        playerAnimations.UpdateMovementAnimation(smoothInputMovement.magnitude);
    }

    // Method to update the player object with data from PlayerData script
    private void UpdatePlayerObject() {
        PlayerData playerData = PlayerData.Instance;
        // Update player's weapon
        weapon = playerData.weapon;
        // Update player's quests
        openQuests = playerData.openQuests;

        currentQuest = playerData.currentQuest;

        // Update player's blessing
        currentBlessing = playerData.blessing;

        // Update player's health
        currentHealth = playerData.activeMaxHealth;

        currentMoveSpeed = playerData.activeSpeed;

    }

    // Method for the player to receive a new quest.
    public void ReceiveNewQuest( QuestSO quest ) {
        // Add the quest to the list of open quests.
        openQuests.Add(quest);
        PlayerData.Instance.UpdateQuests(openQuests);
        // Activate the quest.
        quest.active = true;
        OnQuestActivated?.Invoke();
        // Set the current quest to the received quest.
        currentQuest = quest;
        PlayerData.Instance.UpdateCurrentQuest(quest);
        // Subscribe to the OnQuestCompleted event of the received quest.
        quest.OnQuestCompleted += RemoveCompletedQuest;
    }

    // Removes a completed quest from the list of open quests.
    void RemoveCompletedQuest( QuestSO quest ) {
        // Check if the completed quest is the current quest.
        if (currentQuest == quest) {
            // Reset the current quest to null.
            currentQuest = null;
        }

        // Unsubscribe from the OnQuestCompleted event of the completed quest.
        quest.OnQuestCompleted -= RemoveCompletedQuest;
        // Remove the completed quest from the list of open quests.
        openQuests.Remove(quest);
        PlayerData.Instance.UpdateQuests(openQuests);
        OnReadyToLeave?.Invoke();


        // If there are remaining open quests, set the current quest to the first one in the list.
        if (openQuests.Count > 0) {
            currentQuest = openQuests[0];
            PlayerData.Instance.UpdateCurrentQuest(openQuests[0]);
        }
    }

    // Called automatically by Unity when the script instance is enabled.
    private void OnEnable() {
        // Loop through the list of open quests in reverse order.
        for (int i = openQuests.Count - 1; i >= 0; i--) {
            // Check if the current quest is completed.
            if (openQuests[i].QuestCompleted) {
                // If completed, remove it from the list of open quests.
                RemoveCompletedQuest(openQuests[i]);
            } else {
                // If not completed, subscribe to its OnQuestCompleted event.
                openQuests[i].OnQuestCompleted += RemoveCompletedQuest;
            }
        }
    }

    // Called automatically by Unity when the script instance is disabled.
    void OnDisable() {
        // Unsubscribe from the OnQuestCompleted event of all open quests.
        foreach (QuestSO quest in openQuests) {
            quest.OnQuestCompleted -= RemoveCompletedQuest;
        }
    }
    public Transform GetWeaponFollowTransform() {
        return weaponHoldingPoint;
    }

    public void SetWeapon( Weapon weapon ) {
        this.weapon = weapon;
    }

    public void ClearWeapon() {
        this.weapon = null;
    }

    public bool HasWeapon() {
        return weapon != null;
    }

    public Weapon GetWeapon() {
        return weapon;
    }
}