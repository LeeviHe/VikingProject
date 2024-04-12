using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour, IWeaponParent {
    public float timeBetweenHitDetections;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform weaponHoldingPoint;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;

    public static event Action OnPlayerDeath;

    private Weapon weapon; //Weapon of player
    private PlayerState currentState; //Current state of player action

    private float currentHealth; //Handle player's health
    private float nextAttackTime = 0f; //Time when next attack is allowed
    private float damageCooldown = 2f; // Cooldown duration in seconds
    private float lastDamageTime; // Time when player last took damage
    private float healthLerpSpeed = 0.01f; //Value for healthbar animation speed
    private bool playerAlive = true;

    public LayerMask enemyLayer; // Define the layer for enemy NPCs
    public QuestSO currentQuest; // Active quest for player
    public List<QuestSO> openQuests; //List of all quest set for player
    public PlayerStatsSO stats; //Player stats
    Animator animator; //Player animator

    private void Start() {
        animator = GetComponent<Animator>();
        currentHealth = stats.maxHealth;
        currentState = PlayerState.Idle;
        UpdateHealthUI();
    }

    private Vector3 moveDir;
    private void Update() {
        UpdateHealthUI();
        //Check input for changing player state
        if (gameInput.IsBlocking() && weapon != null) {
            currentState = PlayerState.Blocking;
        } else if (gameInput.AttackInput() && Time.time >= nextAttackTime && weapon != null) {
            currentState = PlayerState.Attacking;
        } else if (gameInput.IsMoving()) {
            currentState = PlayerState.Walking;
        } else if (gameInput.IsIdle()) {
            currentState = PlayerState.Idle;
        }
        //!!!POLISH!!! Might need to cleanup movement handling

        // Get normalized input vector
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // Apply skewing transformation for isometric movement
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedInput = matrix.MultiplyPoint3x4(new Vector3(inputVector.x, 0f, inputVector.y));

        // Calculate movement direction in world space
        moveDir = new Vector3(skewedInput.x, 0f, skewedInput.z).normalized;

        float moveDistance = stats.movementSpeed * Time.deltaTime;
        float playerRadius = .5f;
        float playerHeight = 1f;
        float rotationSpeed = 15f;

        // Check if the player can move in the desired direction
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove) {
            // If can't move in the desired direction, try moving along X or Z axis

            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove) {
                // Can move only on the X axis
                moveDir = moveDirX;

            } else {
                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove) {
                    // Can move only on the Z axis
                    moveDir = moveDirZ;
                } else {
                    currentState = PlayerState.Idle;
                    // Cannot move in any direction
                }
            }
        }

        // Move the player if movement is allowed
        if (canMove) {
            if (moveDir != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(-moveDir, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            // Apply rotation
            //transform.position += moveDir * moveDistance;
            transform.Translate(moveDir * stats.movementSpeed * Time.deltaTime, Space.World);
        }

        HandleState();
    }
    private void HandleState()
    {
        // Handle logic based on current state
        switch (currentState)
        {
            case PlayerState.Idle:
                // Trigger idle animation
                animator.ResetTrigger("Running");
                animator.ResetTrigger("Attack");
                animator.ResetTrigger("Block");
                break;
            case PlayerState.Walking:
                // Trigger walking animation
                animator.SetTrigger("Running");
                animator.ResetTrigger("Attack");
                animator.ResetTrigger("Block");
                break;
            case PlayerState.Blocking:
                // Trigger blocking animation
                Block();
                break;
            case PlayerState.Attacking:
                Attack();
                break;
        }
    }


    public void TakeDamage( float damage ) {
        if (playerAlive) {
            //Check if cooldown for taking damage has passed
            if (Time.time - lastDamageTime >= damageCooldown) {
                //If player is blocking change the damage value based on player weapon blocking power
                if (currentState == PlayerState.Blocking) {
                    damage /= weapon.blockingPower;
                    Debug.Log("Blocked");
                }
                //Substract armor value from damage
                damage = damage - stats.armor;
                Debug.Log("Player took " + damage + " dmg");
                currentHealth -= damage;
                Debug.Log(currentHealth);
                //Perform death when health is depleted
                if (currentHealth <= 0) {
                    HandleDeath();
                } else {
                    animator.SetTrigger("Damage");
                }
                //Update last damage time
                lastDamageTime = Time.time;
            }
        } else {
            Debug.Log("Player already dead");
        }
    }
    public void Attack( ) {
        //Check if player has weapon
        if (weapon != null && !IsAttackAnimationPlaying(animator)) {
            animator.SetTrigger("Attack");
            StartCoroutine(PerformAttack());
        }
    }
    private IEnumerator PerformAttack() {
            yield return new WaitForSeconds(0.5f);
            Debug.Log(IsAttackAnimationPlaying(animator));
            // Track enemies hit during the entire attack animation
            List <Collider> allHitEnemies = new List<Collider>();

            while (IsAttackAnimationPlaying(animator)) {
                // Damage dealt is randomly set on each hit, defined between weapon's damage stats
                int damage = UnityEngine.Random.Range(weapon.minDamage, weapon.maxDamage + 1);
                // Detect enemies in attack range
                Collider[] potentialHitEnemies = Physics.OverlapSphere(weaponHoldingPoint.position, weapon.hitRange, enemyLayer);
                List<Collider> newHitEnemies = new List<Collider>();

                // Filter out enemies that have already been hit during this attack
                foreach (Collider enemy in potentialHitEnemies) {
                    if (!allHitEnemies.Contains(enemy)) {
                        newHitEnemies.Add(enemy);
                    }
                }

                // If new enemies are hit, apply damage to each enemy
                if (newHitEnemies.Count > 0) {
                    // Apply damage to EACH enemy hit
                    foreach (Collider enemy in newHitEnemies) {
                        if (enemy.GetComponent<EnemyNpc>() != null) {
                            Debug.Log("applied " + damage + "to " + enemy);
                            enemy.GetComponent<EnemyNpc>().TakeDamage(damage);
                            allHitEnemies.Add(enemy);
                        }
                    }
                }
                // Wait for the next frame
                yield return new WaitForSeconds(0.2f);
            }
    }
    private bool IsAttackAnimationPlaying( Animator animator ) {
        // Get the current state of the animator
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Assuming the attack animation is in layer 0
        bool isPlaying = stateInfo.IsName("Attack");
        // Check if the animation named "Attack" is still playing
        return isPlaying;
    }
    //Visualizing the attack range in the scene view
    void OnDrawGizmosSelected() {
        if (weapon != null) {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponHoldingPoint.position, weapon.hitRange); 
        }
    }
    public void Block() {
        animator.SetTrigger("Block");
        animator.ResetTrigger("Running");
        animator.ResetTrigger("Attack");
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
        playerAlive = false;
        gameInput.DisableInput();
        animator.SetTrigger("Dying");
        Debug.Log("Player dies, game ends");
    }

    // Method for the player to receive a new quest.
    public void ReceiveNewQuest(QuestSO quest) { 
        // Add the quest to the list of open quests.
        openQuests.Add(quest);
        // Activate the quest.
        quest.active = true;
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