using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    void Start() {
        navAgent = GetComponent<NavMeshAgent>();
        currentHealth = enemyStats.maxHealth;
        navAgent.speed = enemyStats.movementSpeed;
    }

    public void FixedUpdate() {
        withinAggroColliders = Physics.OverlapSphere(transform.position, enemyStats.aggroRange, aggroLayerMask);
        if (withinAggroColliders.Length > 0) {
            ChasePlayer(withinAggroColliders[0].GetComponent<Player>());
        }
        UpdateHealthUI();
    }

    public void TakeDamage( float damage ) {
        damage = damage - enemyStats.armor;
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if (currentHealth <= 0) {
            Die();
        }
    }

    public void PerformAttack() {
        player.TakeDamage(enemyStats.damage);
    }

    void UpdateHealthUI() {
        if (healthSlider.value != currentHealth) {
            healthSlider.value = currentHealth;
        }
        if (healthSlider.value != easeHealthSlider.value) {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, lerpSpeed);
        }
    }

    void ChasePlayer(Player player) { 
        this.player = player;
        navAgent.SetDestination(player.transform.position);
        if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
            if (!IsInvoking("PerformAttack")) {
                InvokeRepeating("PerformAttack", .5f, enemyStats.attackCooldown);
            }
        } else {
            CancelInvoke();
        }
    }

    void Die() {
        // Handle enemy death
        Destroy(gameObject);
        ObjectiveInteraction();
    }
}