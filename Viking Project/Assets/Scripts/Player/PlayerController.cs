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
    public WeaponSO weapon;
    public GameObject weaponObject;
    public Transform weaponHoldingPoint;
    public BlessingSO currentBlessing;
    public int healthPotions;
    // Spawn point of ability 
    // CHANGE!!
    public Transform abilityOrigin;
    [Header("Health Sliders")]
    public float currentHealth; //Handle player's health
    public float currentMoveSpeed;
    [Header("Coins")]
    public int coins;

    [Header("States")]
    public bool isAlive = true;
    public bool isBlocking = false;
    public bool isFightMode = false;
    public bool canWieldTwoHanded;

    [Header("Quests")]
    public QuestSO currentQuest; // Active quest for player
    public List<ObjectiveSO> openQuests = new List<ObjectiveSO>(); //List of all quest set for player

    public PlayerStatsSO stats; //Player stats
    private float nextAttackTime = 0f; //Time when next attack is allowed
    private float attackDefaultCooldown = 2f;

    //public static event Action OnPlayerWin;
    public static event Action OnQuestActivated;
    public static event Action OnReadyToLeave;

    private void Awake() {
        UpdatePlayerObject();
    }

    private void Start() {
        isAlive = true;
        if (weapon) {
            Weapon.SpawnWeapon(weapon, this, weapon.prefab.transform.rotation, this);
        }
    }

    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;
    private Vector2 lookInput;
    [SerializeField] private float movementSmoothingSpeed;

    private void Update() {
        coins = PlayerData.Instance.coins;
        if (currentQuest) {
            openQuests = currentQuest.objectives;
        }
        if (isAlive) {
            CalculateMovementInputSmoothing();
            UpdatePlayerMovement();
            UpdatePlayerAnimationMovement();
        } else {
            playerAnimations.enabled = false;
        }
        if (HasWeapon()) {
            if (weapon.twoHanded && !canWieldTwoHanded && !currentBlessing.canWieldTwoHanded) {
                ClearWeapon();
            }
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

    public void OnHeal( InputAction.CallbackContext value ) {
        if (value.started) {
            if (healthPotions > 0) {
                if (currentHealth < PlayerData.Instance.activeMaxHealth) {
                    float newHealth = currentHealth + 50;
                    if (newHealth >= PlayerData.Instance.activeMaxHealth) {
                        currentHealth = PlayerData.Instance.activeMaxHealth;
                    } else { 
                        currentHealth += 50;
                    }
                    healthPotions--;
                } else {
                    Debug.Log("Player has max health");
                }
            } else {
                Debug.Log("No health potions");
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

    public void OnTogglePause(InputAction.CallbackContext value) {
        if (value.started) {
            Debug.Log("Game pause input pressed");
            //GameManager.Instance.TogglePauseState();
        }
    }

    private void EnterFightMode() {
        // Perform actions to enter fight mode
        playerAnimations.SwitchAttackModeAnimation();
        currentMoveSpeed *= 0.5f;
    }

    // Method to handle exiting fight mode
    public void ExitFightMode() {
        // Perform actions to exit fight mode
        playerAnimations.SwitchAttackModeAnimation();
        currentMoveSpeed = PlayerData.Instance.activeSpeed;
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

    public void RemoveBlessingEffects() {
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

        coins = playerData.coins;

        currentMoveSpeed = playerData.activeSpeed;

        if (currentBlessing) { 
            canWieldTwoHanded = currentBlessing.canWieldTwoHanded;
        }

        healthPotions = playerData.healthPotions;

    }

    // Method for the player to receive a new quest.
    public void ReceiveNewQuest( QuestSO quest ) {
        // Activate the quest.
        quest.active = true;
        OnQuestActivated?.Invoke();
        // Set the current quest to the received quest.
        currentQuest = quest;
        PlayerData.Instance.UpdateCurrentQuest(quest);
        // Subscribe to the OnQuestCompleted event of the received quest.
        Debug.Log("subscribed to " + quest);
        quest.OnQuestCompleted += RemoveCompletedQuest;
    }

    // Removes a completed quest from the list of open quests.
    void RemoveCompletedQuest( QuestSO quest ) {
        // Check if the completed quest is the current quest.
        if (currentQuest == quest) {
            currentQuest = null;
            PlayerData.Instance.UpdateCurrentQuest(currentQuest);
            // Unsubscribe from the OnQuestCompleted event of the completed quest.
            quest.OnQuestCompleted -= RemoveCompletedQuest;
            
            OnReadyToLeave?.Invoke();
        }
    }

    // Called automatically by Unity when the script instance is enabled.
    private void OnEnable() {
        if (currentQuest) { 
            currentQuest.OnQuestCompleted += RemoveCompletedQuest;
        }
    }

    // Called automatically by Unity when the script instance is disabled.
    void OnDisable() {
        if (currentQuest) { 
            currentQuest.OnQuestCompleted -= RemoveCompletedQuest;
        }
    }
    public Transform GetWeaponFollowTransform() {
        return weaponHoldingPoint;
    }

    public void SetWeapon( WeaponSO weapon, GameObject weaponObject ) {
        this.weapon = weapon;
        this.weaponObject = weaponObject;
    }

    public void ClearWeapon() {
        Destroy(weaponObject);
        weapon = null;
        weaponObject = null;
    }

    public bool HasWeapon() {
        return weapon != null;
    }

    public GameObject GetWeapon() {
        return weaponObject;
    }
}