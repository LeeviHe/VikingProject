using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : QuestItem {
    private int maxHealth = 100;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    private float lerpSpeed = 0.01f;
    int currentHealth;

    void Start() {
        currentHealth = maxHealth;
    }

    public void Update() {
        UpdateHealthUI();
    }

    public void TakeDamage( int damage ) {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if (currentHealth <= 0) {
            Die();
        }
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
        // Handle enemy death
        Destroy(gameObject);
        ObjectiveInteraction();
    }
}