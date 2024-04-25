using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
    [SerializeField] private PlayerStatsSO stats;
    public static PlayerData Instance { get; private set; }

    [Header("Weapon")]
    public Weapon weapon;

    [Header("Player Quests")]
    public QuestSO currentQuest; // Active quest for player
    public List<QuestSO> openQuests; //List of all quests set for player

    [Header("Blessing")]
    public BlessingSO blessing;

    [Header("Health")]
    public float activeMaxHealth;

    [Header("Speed")]
    public float activeSpeed;
    private void Awake() {
        if (Instance == null) {
            activeMaxHealth = stats.maxHealth;
            activeSpeed = stats.movementSpeed;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    public void UpdateWeapon( Weapon newWeapon ) {
        weapon = newWeapon;
        // Optionally, update the player object with the new weapon
    }

    // Method to update the player's quests
    public void UpdateCurrentQuest( QuestSO updatedCurrentQuest ) {
        currentQuest = updatedCurrentQuest;
        // Optionally, update the player object with the new quests
    }
    // Method to update the player's quests
    public void UpdateQuests( List<QuestSO> updatedQuests ) {
        openQuests = updatedQuests;
        // Optionally, update the player object with the new quests
    }

    // Method to update the player's blessing
    public void UpdateBlessing( BlessingSO newBlessing ) {
        blessing = newBlessing;
        // Optionally, update the player object with the new blessing
    }

    // Method to update the player's health
    public void UpdateHealth( float newHealth ) {
        activeMaxHealth = newHealth;
        // Optionally, update the player object with the new health
    }
    public void UpdateSpeed( float newSpeed ) {
        activeSpeed = newSpeed;
        // Optionally, update the player object with the new health
    }
}