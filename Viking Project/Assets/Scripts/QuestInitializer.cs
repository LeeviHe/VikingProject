using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInitializer : MonoBehaviour{
    public GameObject questItemPrefab;
    public QuestSO quest;
    void Start() {
        InitializeQuest();
    }
    void InitializeQuest() {
        // Example: Add objectives to the quest
        // You can call AddObjective() multiple times to add different objectives
        // Example:
        // AddObjective(objective1);
        // AddObjective(objective2);
        foreach (var objective in quest.objectives) {
            // Instantiate a new GameObject for each quest objective
            GameObject questItemGO = Instantiate(questItemPrefab, Vector3.zero, Quaternion.identity);
            // Get the QuestItem component attached to the newly instantiated GameObject
            QuestItem questItem = questItemGO.GetComponent<QuestItem>();
            // Set the objective for the QuestItem
            questItem.SetObjective(objective);
        }

    }
}
