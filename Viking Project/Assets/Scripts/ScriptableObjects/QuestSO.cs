using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class QuestSO : ScriptableObject {
    public event Action<QuestSO> OnQuestCompleted;
    public bool active;
    public bool QuestCompleted { get; private set; }
    [TextArea]
    public string questDescription;

    //List of objectives for the quest set in the Editor
    //Here you put objectives you want the script to clone in initialization
    public List<ObjectiveSO> objectives = new List<ObjectiveSO>();

    //Try to complete quest
    public void TryEndQuest() {
        // Check if quest has no objectives set to it
        if (objectives == null || objectives.Count == 0) {
            Debug.LogError("No objectives found for the quest.");
            return;
        }

        // Check if all required objectives are completed

        // Start with true value
        bool allRequiredObjectivesCompleted = true;
        // Loop through objectives
        foreach (var objective in objectives) {
            Debug.Log("Objectives : " + objective.description + " status : " + objective.Completed);
            // If iterated objective in the loop is required and is not completed set requirement value as false and break off from loop
            if (objective.required && !objective.Completed) {
                allRequiredObjectivesCompleted = false;
                Debug.Log("Quest still active: " + questDescription);
                Debug.Log("Objective left: " + objective.description);
                break;
            }
        }
        // If all required objectives are completed, mark the quest as completed, make it inactive and Invoke OnQuestCompleted event
        if (allRequiredObjectivesCompleted) {

            Debug.Log("Quest Completed!");
            QuestCompleted = true;
            active = false;
            OnQuestCompleted?.Invoke(this);
        }
    }
}