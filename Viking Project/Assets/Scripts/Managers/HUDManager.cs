using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public PlayerController playerController;
    [Header("UI Elements")]
    [SerializeField] private GameObject questUI;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI questTitle;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private Toggle questStatus;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;

    public List<ObjectiveSO> objectives = new List<ObjectiveSO>();

    public List<Toggle> objectiveUIs = new List<Toggle>();

    public List<Text> objectiveLabelObjects = new List<Text>();

    public void Update() {
        RefreshHUDInfo();
        UpdateHealthUI();
    }

    void SetObjectiveUI() {
        for (int i = 0; i < objectives.Count; i++) {
            objectiveUIs[i].gameObject.SetActive(true);
            objectiveLabelObjects[i].text = objectives[i].description;
            objectiveUIs[i].isOn = objectives[i].Completed;
        }
        for (int i = objectives.Count; i < objectiveUIs.Count; i++) {
            objectiveUIs[i].gameObject.SetActive(false);
        }
    }

    public void RefreshHUDInfo() { 
        string description;
        string header;
        if (playerController.currentQuest) {
            questUI.SetActive(true);
            objectives = playerController.currentQuest.objectives;
            SetObjectiveUI();
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
        if (healthSlider.value > playerController.currentHealth) {
            healthSlider.value = playerController.currentHealth;
        } else if (healthSlider.value < playerController.currentHealth) {
            healthSlider.value = Mathf.Lerp(healthSlider.value, playerController.currentHealth, healthLerpSpeed * 2);
        }
        //For nice animation
        if (healthSlider.value != easeHealthSlider.value) {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, playerController.currentHealth, healthLerpSpeed * 1.5f);
        }
    }
}
