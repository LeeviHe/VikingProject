using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public PlayerController playerController;
    [Header("UI Elements")]
    [SerializeField] private GameObject questUI;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI questTitle;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private Toggle questStatus;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;

    public void Update() {
        RefreshHUDInfo();
        UpdateHealthUI();
    }

    public void RefreshHUDInfo() { 
        string description;
        string header;
        if (playerController.currentQuest) {
            questUI.SetActive(true);
            description = playerController.currentQuest.questDescription;
            header = playerController.currentQuest.name;
        } else {
            questUI.SetActive(false);
            description = "";
            header = "";
        }
        questTitle.text = header;
        questDescription.text = description;
    }
    void UpdateHealthUI() {
        PlayerData playerData = PlayerData.Instance;
        float healthLerpSpeed = 0.01f; //Value for healthbar animation speed
        healthSlider.maxValue = playerData.activeMaxHealth;
        easeHealthSlider.maxValue = playerData.activeMaxHealth;
        //If health value changes, update healthslider to new value
        if (healthSlider.value != playerController.currentHealth) {
            healthSlider.value = playerController.currentHealth;
        }
        //For nice animation
        if (healthSlider.value != easeHealthSlider.value) {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, playerController.currentHealth, healthLerpSpeed);
        }
    }
}
