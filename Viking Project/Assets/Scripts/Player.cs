using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IWeaponParent {
    public float timeBetweenHitDetections;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform weaponHoldingPoint;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    public static event Action OnPlayerDeath;
    public static event Action OnPlayerWin;
    public static event Action OnQuestActivated;
    public static event Action OnReadyToLeave;

    private Weapon weapon; //Weapon of player
    private PlayerState currentState; //Current state of player action

    private float currentHealth; //Handle player's health
    private float nextAttackTime = 0f; //Time when next attack is allowed
    private float attackDefaultCooldown = 2f;
    private float damageCooldown = 2f; // Cooldown duration in seconds
    private float lastDamageTime; // Time when player last took damage
    private float healthLerpSpeed = 0.01f; //Value for healthbar animation speed
    private bool playerAlive = true;
    private float attackDuration;

    public LayerMask enemyLayer; // Define the layer for enemy NPCs
    public QuestSO currentQuest; // Active quest for player
    public List<QuestSO> openQuests; //List of all quest set for player
    public PlayerStatsSO stats; //Player stats
    public Animator playerAnimator; //Player animator

    private void Awake() {
        playerAnimator = GetComponent<Animator>();
    }
    private void Start() {
        playerAlive = true;
        currentHealth = stats.maxHealth;
        currentState = PlayerState.Idle;
        UpdateHealthUI();
    }

    private Vector3 moveDir;
    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;
    [SerializeField] private float movementSmoothingSpeed;

    private void Update() {
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();
        UpdatePlayerAnimationMovement();
        MovePlayer();
        TurnPlayer();

        Debug.Log(currentState);
        UpdateHealthUI();
        if (playerAlive) { 
            HandleState();
        }
        if (gameInput.IsIdle()) {
            currentState = PlayerState.Idle;
        }
    }
    private void HandleState() {
        // Handle logic based on current state

        switch (currentState) {
            case PlayerState.Idle:
                break;
            case PlayerState.Moving:
                break;
            case PlayerState.Blocking:
                break;
            case PlayerState.Attacking:
                break;
            default:
                Debug.Log("unhandled state");
                break;
        }
    }


    public void PlayerMovement( InputAction.CallbackContext value ) {
        if (currentState != PlayerState.Blocking) { 
            currentState = PlayerState.Moving;
        }
        // Get normalized input vector
        Vector2 inputVector = value.ReadValue<Vector2>();

        // Apply skewing transformation for isometric movement
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedInput = matrix.MultiplyPoint3x4(new Vector3(inputVector.x, 0f, inputVector.y));

        // Calculate movement direction in world space
        rawInputMovement = new Vector3(skewedInput.x, 0f, skewedInput.z).normalized;
    }
    void UpdateMovementData( Vector3 newMovementDirection ) {
        moveDir = newMovementDirection;
    }
    void CalculateMovementInputSmoothing() {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);

    }
    void UpdatePlayerMovement() {
        UpdateMovementData(smoothInputMovement);
    }
    public void UpdateMovementAnimation( float movementBlendValue ) {
        playerAnimator.SetFloat("Movement", movementBlendValue);
    }
    void UpdatePlayerAnimationMovement() {
        UpdateMovementAnimation(smoothInputMovement.magnitude);
    }
    public void MovePlayer() {
        float moveDistance = stats.movementSpeed * Time.deltaTime;
        if (currentState == PlayerState.Blocking) {
            moveDistance *= 0.3f;
        }
        transform.Translate(moveDir * moveDistance, Space.World);
    }

    public void TurnPlayer() {
        float rotationSpeed = 10f; 
        if (moveDir.sqrMagnitude > 0.01f) {
            Quaternion targetRotation = Quaternion.LookRotation(-moveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    public void OnAttack( InputAction.CallbackContext value ) {
        if (value.started && weapon != null && attackDuration <= 0 && currentState != PlayerState.Blocking) {
            playerAnimator.SetTrigger("Attack");
            nextAttackTime = Time.time + attackDefaultCooldown / weapon.attackSpeed;
            attackDuration = (nextAttackTime - Time.time);
            StartCoroutine(PerformAttack());
        }
    }
    public void TakeDamage( float damage ) {
        if (playerAlive) {
            //Check if cooldown for taking damage has passed

            //If player is blocking change the damage value based on player weapon blocking power
            if (currentState == PlayerState.Blocking) {
                damage /= weapon.blockingPower;
                Debug.Log("Blocked");
            }
            //Substract armor value from damage
            damage = damage - stats.armor;
            Debug.Log("Player took " + damage + " dmg");

            //Perform death when health is depleted
            if (Time.time - lastDamageTime >= damageCooldown) {
                currentHealth -= damage;
                Debug.Log(currentHealth);
            } else {
                damage *= 0.2f;
                currentHealth -= damage;
            }
            if (currentHealth <= 0) {
                HandleDeath();
            } else {
                playerAnimator.SetTrigger("Damage");
            }
            //Update last damage time
            lastDamageTime = Time.time;
        } else {
            Debug.Log("Player already dead");
        }
    }


    private IEnumerator PerformAttack() {
        playerAnimator.speed = 1.0f / attackDuration;
        yield return new WaitForSeconds(attackDuration * 0.8f);
        // Track enemies hit during the entire attack animation
        List<Collider> allHitEnemies = new List<Collider>();
        Debug.Log("start detection");
        while (attackDuration > 0) {
            currentState = PlayerState.Attacking;
            // Detect enemies in attack range
            Collider[] potentialHitEnemies = Physics.OverlapSphere(weaponHoldingPoint.position, weapon.hitRange, enemyLayer);
            foreach (Collider enemy in potentialHitEnemies) {
                if (!allHitEnemies.Contains(enemy)) {
                    // Apply damage to the enemy
                    if (enemy.GetComponent<EnemyNpc>() != null) {
                        // Damage dealt is randomly set on each hit, defined between weapon's damage stats
                        int damage = UnityEngine.Random.Range(weapon.minDamage, weapon.maxDamage + 1);
                        Debug.Log("Applied " + damage + " to " + enemy);
                        enemy.GetComponent<EnemyNpc>().TakeDamage(damage);
                        allHitEnemies.Add(enemy);
                    }
                }
            }
            attackDuration -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Stop detection");
        playerAnimator.speed = 1f;
        currentState = PlayerState.Idle;
    }
    //Visualizing the attack range in the scene view
    void OnDrawGizmosSelected() {
        if (weapon != null) {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponHoldingPoint.position, weapon.hitRange); 
        }
    }
    public void OnBlock( InputAction.CallbackContext value ) {
        if (weapon != null) {
            if (value.started) {
                playerAnimator.SetBool("Block", true);
                currentState = PlayerState.Blocking;
            } else if (value.canceled) {
                playerAnimator.SetBool("Block", false);
                currentState = PlayerState.Idle;
            }
        }
    }

    //Function to update health bars
    void UpdateHealthUI() {
        healthSlider.maxValue = stats.maxHealth;
        easeHealthSlider.maxValue = stats.maxHealth;
        //If health value changes, update healthslider to new value
        if (healthSlider.value != currentHealth) {
            healthSlider.value = currentHealth;
        }
        //For nice animation
        if (healthSlider.value != easeHealthSlider.value) {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, healthLerpSpeed);
        }
    }


    //Handle death
    void HandleDeath() {
        playerAnimator.SetTrigger("Dying");
        OnPlayerDeath?.Invoke();
        playerAlive = false;
        gameInput.DisableInput();
        Debug.Log("Player dies, game ends");
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
    void RemoveCompletedQuest(QuestSO quest) {
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