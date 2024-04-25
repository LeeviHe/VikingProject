using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AI;
using UnityEngine.EventSystems;


//Take QuestItem interaction to use here, 
//!!!POLISH!!! Make new IEnemy for use here
public class EnemyNpc : QuestItem {
    public float timeBetweenHitDetections;
    public LayerMask aggroLayerMask;
    public NPCStatsSO enemyStats;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    [SerializeField] private Transform weaponHoldingPoint;
    private NavMeshAgent navAgent;
    private PlayerController player;
    private Collider[] withinAggroColliders;
    private float lerpSpeed = 0.01f;
    private bool npcAlive = true;
    float currentHealth;
    [SerializeField] private Animator animator;
    EnemyState currentState;

    //Assign navAgent to be NavMeshAgent component of this enemy instance, and assign NPC stats
    void Start() {
        currentState = EnemyState.Idle;
        navAgent = GetComponent<NavMeshAgent>();
        currentHealth = enemyStats.maxHealth;
        navAgent.speed = enemyStats.movementSpeed;
        UpdateHealthUI();
    }

    //FixedUpdate or normal update? Check performance
    public void Update() {
        if (npcAlive) { 
            //Set Aggro area with NPC aggro value and layer that is to be aggroed (player)
            withinAggroColliders = Physics.OverlapSphere(transform.position, enemyStats.aggroRange, aggroLayerMask);
            //If any aggroLayer object is within range perform ChasePlayer function
            //Only 1 object that is the player is supposed to be detectable as within range
            if (withinAggroColliders.Length > 0) {
                ChasePlayer(withinAggroColliders[0].GetComponent<PlayerController>());
            } else {
                currentState = EnemyState.Idle;
            }
            HandleState();
        }
        UpdateHealthUI();
        
    }

    private void HandleState() {
        // Handle logic based on current state
        switch (currentState) {
            case EnemyState.Idle:
                // Trigger idle animation
                animator.ResetTrigger("Running");
                break;
            case EnemyState.Walking:
                // Trigger walking animation
                animator.SetTrigger("Running");
                break;
        }
    }

    //NPC takes damage, armor value is substracted and then damage is dealt. Perform death function after no more health
    public void TakeDamage( float damage ) {
        if (npcAlive) {
            damage = damage - enemyStats.armor;
            currentHealth -= damage;
            if (currentHealth <= 0) {
                Die();
            }
        }
    }

    //Deal damage to player with NPC stat value
    public void EnemyAttack() {
        //Debug.Log("Attack performed");
        StartCoroutine(PerformEnemyAttack());
    }

    private IEnumerator PerformEnemyAttack() {
        
        animator.SetTrigger("Attack");
        HashSet<Collider> hitPlayers = new HashSet<Collider>();
        yield return new WaitForSeconds(1f); // Delay before attacking
        //Damage dealt is randomly set on each hit, defined between weapon's damage stats
        int damage = UnityEngine.Random.Range(enemyStats.minDamage, enemyStats.maxDamage + 1);
        // Detect enemies in attack range
        Collider[] potentialHitPlayers = Physics.OverlapSphere(weaponHoldingPoint.position, enemyStats.attackRange, aggroLayerMask);
        // Check if player has been hit already
        foreach (Collider playerCollider in potentialHitPlayers) {
            if (!hitPlayers.Contains(playerCollider)) {
                // If player hasn't been hit, apply damage and mark as hit
                playerCollider.GetComponent<PlayerCombat>().TakeDamage(damage);
                hitPlayers.Add(playerCollider);
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponHoldingPoint.position, enemyStats.attackRange);
    }

    //Function to update health bars
    void UpdateHealthUI() {
        healthSlider.maxValue = enemyStats.maxHealth;
        easeHealthSlider.maxValue = enemyStats.maxHealth;
        //If health value changes, update healthslider to new value
        if (healthSlider.value != currentHealth) {
            healthSlider.value = currentHealth;
        }
        //For nice animation
        if (healthSlider.value != easeHealthSlider.value) {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, lerpSpeed);
        }
    }

    //Chase function, perform only when within aggro range
    void ChasePlayer(PlayerController player) {
        if (npcAlive) {
            //If npc is alive
            this.player = player;
            //Set destination of NPC to player's position
            navAgent.SetDestination(player.transform.position);
            currentState = EnemyState.Walking;
            // Check if NPC close enough to player, then perform attacks between NPC stat attackCooldown intervals, otherwise stop performing attack
            //!!!POLISH!!! See if there is a cleaner and more fitting way to do this
            if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
                //In Attack Range
                //Check if already invoking attack
                if (!IsInvoking("EnemyAttack")) {
                    InvokeRepeating("EnemyAttack", .1f, enemyStats.attackCooldown);
                }
            } else {
                //No longer in attack range
                //Canvel attack invoking
                CancelInvoke("EnemyAttack");
            }
        } else {
            //If npc is dead
            CancelInvoke("EnemyAttack");
        }
        
    }
    
    // Handle enemy death
    void Die() {
        Debug.Log("NPC died");
        npcAlive = false;
        animator.SetTrigger("Dying");
        animator.ResetTrigger("Running");
        animator.ResetTrigger("Attack");
        CancelInvoke("EnemyAttack");
        Destroy(gameObject, 5f);
        // If NPC death is objective perform interaction with objective
        if (objective) { 
            ObjectiveInteraction();
        }
    }
}