using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestSO : ScriptableObject {
    public event Action<QuestSO> OnQuestCompleted;
    public bool active;
    public bool QuestCompleted { get; private set; }
    [TextArea]
    public string questDescription;

    public List<ObjectiveSO> objectives = new List<ObjectiveSO>();

    public void TryEndQuest() {
        Debug.Log("try end quest");
        if (objectives == null || objectives.Count == 0) {
            Debug.LogError("No objectives found for the quest.");
            return;
        }

        // Check if all required objectives are completed
        bool allRequiredObjectivesCompleted = true;
        foreach (var objective in objectives) {
            Debug.Log("Objectives : " + objective.description + " status : " + objective.Completed);
            if (objective.required && !objective.Completed) {
                allRequiredObjectivesCompleted = false;
                Debug.Log("Quest still active: " + questDescription);
                Debug.Log("Objectives left: " + objective.description);
                break;
            }
        }

        if (allRequiredObjectivesCompleted) {
            // If all required objectives are completed, mark the quest as completed
            Debug.Log("Quest Completed!");
            QuestCompleted = true;
            active = false;
            OnQuestCompleted?.Invoke(this);
        }
    }

    void OnEnable() {
        for (int i = 0; i < objectives.Count; i++) { 
            objectives[i].parentQuest = this;
        }
    }
}
