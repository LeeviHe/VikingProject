using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public int maxHealth = 100;
    int currentHealth;

    void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage( int damage ) {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        // Handle enemy death
        Destroy(gameObject);
    }
}