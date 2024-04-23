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

    [Header("Health Sliders")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;

    [Header("States")]
    public bool isAlive = true;
    public bool isBlocking = false;

    [Header("Quests")]
    public QuestSO currentQuest; // Active quest for player
    public List<QuestSO> openQuests; //List of all quest set for player

    public PlayerStatsSO stats; //Player stats
    private float nextAttackTime = 0f; //Time when next attack is allowed
    private float attackDefaultCooldown = 2f;
    private float healthLerpSpeed = 0.01f; //Value for healthbar animation speed

    public static event Action OnPlayerWin;
    public static event Action OnQuestActivated;
    public static event Action OnReadyToLeave;

    private void Start() {
        isAlive = true;
        UpdateHealthUI();
    }

    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;
    [SerializeField] private float movementSmoothingSpeed;

    private void Update() {
        if (isAlive) {
            CalculateMovementInputSmoothing();
            UpdatePlayerMovement();
            UpdatePlayerAnimationMovement();
            UpdateHealthUI();
        } else { 
            playerAnimations.enabled = false;
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
        if (value.started && weapon != null && playerCombat.attackDuration <= 0) {
            playerAnimations.PlayAttackAnimation();
            nextAttackTime = Time.time + attackDefaultCooldown / weapon.attackSpeed;
            playerCombat.attackDuration = (nextAttackTime - Time.time);
            StartCoroutine(playerCombat.PerformAttack());
        }
    }
    
    public void OnBlock( InputAction.CallbackContext value ) {
        if (weapon != null) {
            if (value.started) {
                playerAnimations.PlayBlockAnimation(true);
                isBlocking = true;
            } else if (value.canceled) {
                playerAnimations.PlayBlockAnimation(false);
                isBlocking = false;
            }
        }
    }

    void CalculateMovementInputSmoothing() {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

    void UpdatePlayerMovement() {
        playerMovement.UpdateMovementData(smoothInputMovement);
    }

    void UpdatePlayerAnimationMovement() {
        playerAnimations.UpdateMovementAnimation(smoothInputMovement.magnitude);
    }

    //Function to update health bars
    void UpdateHealthUI() {
        healthSlider.maxValue = stats.maxHealth;
        easeHealthSlider.maxValue = stats.maxHealth;
        //If health value changes, update healthslider to new value
        if (healthSlider.value != playerCombat.currentHealth) {
            healthSlider.value = playerCombat.currentHealth;
        }
        //For nice animation
        if (healthSlider.value != easeHealthSlider.value) {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, playerCombat.currentHealth, healthLerpSpeed);
        }
    }

    private void OnTriggerEnter( Collider other ) {
        if (other.gameObject.CompareTag("Finish")) {
            OnPlayerWin?.Invoke();
            //gameObject.SetActive(false);
        } else {
            Debug.Log("Empty trigger");
        }
    }

    // Method for the player to receive a new quest.
    public void ReceiveNewQuest( QuestSO quest ) {
        // Add the quest to the list of open quests.
        openQuests.Add(quest);
        // Activate the quest.
        quest.active = true;
        OnQuestActivated?.Invoke();
        // Set the current quest to the received quest.
        currentQuest = quest;

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
        OnReadyToLeave?.Invoke();

        // If there are remaining open quests, set the current quest to the first one in the list.
        if (openQuests.Count > 0) {
            currentQuest = openQuests[0];
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