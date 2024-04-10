using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//Take QuestItem interaction to use here, 
//!!!POLISH!!! Make new IEnemy for use here
public class EnemyNpc : QuestItem {

    public LayerMask aggroLayerMask;
    public NPCStatsSO enemyStats;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    private NavMeshAgent navAgent;
    private Player player;
    private Collider[] withinAggroColliders;
    private float lerpSpeed = 0.01f;
    float currentHealth;

    //Assign navAgent to be NavMeshAgent component of this enemy instance, and assign NPC stats
    void Start() {
        navAgent = GetComponent<NavMeshAgent>();
        currentHealth = enemyStats.maxHealth;
        navAgent.speed = enemyStats.movementSpeed;
    }

    //FixedUpdate or normal update? Check performance
    public void FixedUpdate() {
        //Set Aggro area with NPC aggro value and layer that is to be aggroed (player)
        withinAggroColliders = Physics.OverlapSphere(transform.position, enemyStats.aggroRange, aggroLayerMask);
        //If any aggroLayer object is within range perform ChasePlayer function
        //Only 1 object that is the player is supposed to be detectable as within range
        if (withinAggroColliders.Length > 0) {
            ChasePlayer(withinAggroColliders[0].GetComponent<Player>());
        }
        UpdateHealthUI();
    }

    //NPC takes damage, armor value is substracted and then damage is dealt. Perform death function after no more health
    public void TakeDamage( float damage ) {
        damage = damage - enemyStats.armor;
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
        }
    }

    //Deal damage to player with NPC stat value
    public void PerformAttack() {
        player.TakeDamage(enemyStats.damage);
    }

    //Function to update health bars
    void UpdateHealthUI() {
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
    void ChasePlayer(Player player) { 
        this.player = player;
        //Set destination of NPC to player's position
        navAgent.SetDestination(player.transform.position);
        // Check if NPC close enough to player, then perform attacks between NPC stat attackCooldown intervals, otherwise stop performing attack
        //!!!POLISH!!! See if there is a cleaner and more fitting way to do this
        if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
            if (!IsInvoking("PerformAttack")) {
                InvokeRepeating("PerformAttack", .5f, enemyStats.attackCooldown);
            }
        } else {
            CancelInvoke();
        }
    }
    
    // Handle enemy death
    void Die() {
        Destroy(gameObject);
        // If NPC death is objective perform interaction with objective
        if (objective) { 
            ObjectiveInteraction();
        }
    }
}