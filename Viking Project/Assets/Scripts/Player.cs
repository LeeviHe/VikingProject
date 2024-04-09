using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using UnityEngine.UI;

public class Player : MonoBehaviour, IWeaponParent {

    [SerializeField] private float moveSpeed;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform weaponHoldingPoint;
    [SerializeField] private GameObject weaponObject;
    private Weapon weapon;
    private PlayerState currentState;
    public float maxHealth;
    public float playerArmor;
    private float currentHealth;
    private float nextAttackTime = 0f;
    public LayerMask enemyLayer; // Define the layer for enemy NPCs
    public float damageCooldown = 1f; // Cooldown duration in seconds
    private float lastDamageTime; // Time when player last took damage
    //Reference to player animator
    Animator animator;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    private float lerpSpeed = 0.01f;


    private void Start() {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        //GameObject weaponInstance = Instantiate(weaponObject, weaponHoldingPoint.position, weaponHoldingPoint.rotation);

        // Make the weapon instance a child of the hand to keep it attached
        //weaponInstance.transform.parent = weaponHoldingPoint;
    }

    private void Update() {

        UpdateHealthUI();

        if (Input.GetKey("q")) {
            TakeDamage(25); //Test damage amount
        }

        if (gameInput.IsAttacking() && Time.time >= nextAttackTime && weapon != null) {
            currentState = PlayerState.Attacking;
            nextAttackTime = Time.time + weapon.attackSpeed; // Set the next allowed attack time
        } else if (gameInput.IsBlocking()) {
            currentState = PlayerState.Blocking;
        } else  if (gameInput.IsMoving()){
            currentState = PlayerState.Walking;
        }   else {
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
                    currentState = PlayerState.Idle;
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
            case PlayerState.Attacking:
                // Trigger attacking animation
                animator.SetTrigger("Attack");
                animator.ResetTrigger("Running");
                animator.ResetTrigger("Block");
                Attack();
                break;
            case PlayerState.Blocking:
                // Trigger blocking animation
                animator.SetTrigger("Block");
                animator.ResetTrigger("Running");
                animator.ResetTrigger("Attack");
                Block();
                break;
            default:
                break;
        }
    }


    public void TakeDamage( float damage ) {
        //Check if cooldown has passed
        if (Time.time - lastDamageTime >= damageCooldown) {
            if (currentState == PlayerState.Blocking) {
                
                damage /= 2;
                Debug.Log("blocked");
            }
            damage = damage - playerArmor;
            Debug.Log("took " + damage + " dmg");
            currentHealth -= damage;
            Debug.Log(currentHealth);
            if (currentHealth <= 0) {
                Die();
            }
            //Update last damage time
            lastDamageTime = Time.time;
        }
    }
    public void Attack( ) {
        if (weapon != null) {
            int damage = Random.Range(weapon.minDamage, weapon.maxDamage + 1);
            //animator.SetTrigger("Attack");

            // Detect enemies in attack range
            Collider[] hitEnemies = Physics.OverlapSphere(weaponHoldingPoint.position, weapon.hitRange, enemyLayer);
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
        } else {
            Debug.Log("No weapon equipped");
        }
    }
    //Visualizing the attack range in the scene view
    void OnDrawGizmosSelected() {
        if (weapon != null) {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponHoldingPoint.position, weapon.hitRange); 
        }
        
    }
    public void Block() {
        //Any blocking logic
    }

    void UpdateHealthUI() {
        if (healthSlider.value != currentHealth) {
            healthSlider.value = currentHealth;
        }
        if (healthSlider.value != easeHealthSlider.value) {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, lerpSpeed);
        }
    }
    void Die() {
        Destroy(gameObject);
    }

    public Transform GetWeaponFollowTransform() {
        return weaponHoldingPoint;
    }

    public void SetWeapon( Weapon weapon ) {
        this.weapon = weapon;
    }

    public void ClearWeapon() {
        weapon = null;
    }

    public bool HasWeapon() {
        return weapon != null;
    }

    public Weapon GetWeapon() {
        return weapon;
    }
}