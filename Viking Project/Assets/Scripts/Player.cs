using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class Player : MonoBehaviour {

    [SerializeField] private float moveSpeed;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform weaponHoldingPoint;
    [SerializeField] private GameObject weaponObject;
    private PlayerState currentState;
    public int maxHealth = 100;
    public int playerArmor = 5;
    private int currentHealth;
    public int minAttackDamage = 8;
    public int maxAttackDamage = 11;
    public float attackRange = 1f;
    public float attackCooldown = 0.5f; // Adjust as needed
    private float nextAttackTime = 0f;
    public LayerMask enemyLayer; // Define the layer for enemy NPCs
    public float damageCooldown = 1f; // Cooldown duration in seconds
    private float lastDamageTime; // Time when player last took damage
    //Reference to player animator
    Animator animator;


    private void Start() {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        GameObject weaponInstance = Instantiate(weaponObject, weaponHoldingPoint.position, weaponHoldingPoint.rotation);

        // Make the weapon instance a child of the hand to keep it attached
        weaponInstance.transform.parent = weaponHoldingPoint;
    }

    private void Update() {

        if (Input.GetKey("q")) {
            TakeDamage(25); //Test damage amount
        }

        if (gameInput.IsAttacking() && Time.time >= nextAttackTime) {
            currentState = PlayerState.Attacking;
            nextAttackTime = Time.time + attackCooldown; // Set the next allowed attack time
        } else if (gameInput.IsBlocking()) {
            currentState = PlayerState.Blocking;
        } else {
            currentState = PlayerState.Idle;
        }

        // Get normalized input vector
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // Apply skewing transformation for isometric movement
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedInput = matrix.MultiplyPoint3x4(new Vector3(inputVector.x, 0f, inputVector.y));

        // Calculate movement direction in world space
        Vector3 moveDir = new Vector3(skewedInput.x, 0f, skewedInput.z).normalized;

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .5f;
        float playerHeight = 1f;

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
                    // Cannot move in any direction
                }
            }
        }

        // Move the player if movement is allowed
        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        //Weapon holding
        if (weaponHoldingPoint != null && weaponObject != null) {
            weaponObject.transform.position = weaponHoldingPoint.position;
            weaponObject.transform.rotation = weaponHoldingPoint.rotation;
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
                animator.SetTrigger("Running");
                animator.ResetTrigger("Attack");
                animator.ResetTrigger("Block");
                break;
            case PlayerState.Walking:
                // Trigger walking animation
                animator.SetTrigger("Running");
                animator.ResetTrigger("Attack");
                animator.ResetTrigger("Block");
                break;
            case PlayerState.Attacking:
                // Trigger attacking animation
                animator.SetTrigger("Attack");
                animator.ResetTrigger("Running");
                animator.ResetTrigger("Block");
                break;
            case PlayerState.Blocking:
                // Trigger blocking animation
                animator.SetTrigger("Block");
                animator.ResetTrigger("Running");
                animator.ResetTrigger("Attack");
                break;
            default:
                break;
        }
    }


    public void TakeDamage( int damage ) {
        //Check if cooldown has passed
        if (Time.time - lastDamageTime >= damageCooldown) {
            if (currentState == PlayerState.Blocking) {
                
                damage /= 2;
                Debug.Log("blocked");
            }
            damage = damage - playerArmor;
            Debug.Log("took " + damage + " dmg");
            currentHealth -= damage;
            UpdateHealthUI();
            Debug.Log(currentHealth);
            if (currentHealth <= 0) {
                Die();
            }
            //Update last damage time
            lastDamageTime = Time.time;
        }
    }
    public void Attack() {
        int damage = Random.Range(minAttackDamage, maxAttackDamage + 1);
        //animator.SetTrigger("Attack");

        // Detect enemies in attack range
        Collider[] hitEnemies = Physics.OverlapSphere(weaponHoldingPoint.position, attackRange, enemyLayer);
        if (hitEnemies.Length > 0) {
            Debug.Log("Hit!");
        } else {
            Debug.Log("Miss");
        }
        //Apply damage to EACH enemy hit
        foreach (Collider enemy in hitEnemies) {
            if (enemy.GetComponent<EnemyHealth>() == null) {
                Debug.Log("No script!");
            } else {
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
    }
    //Visualizing the attack range in the scene view
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponHoldingPoint.position, attackRange);
    }
    public void Block() {
        //Any blocking logic
    }

    void UpdateHealthUI() {
        // UI element update
    }
    void Die() {
        Destroy(gameObject);
    }
}