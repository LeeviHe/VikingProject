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
    // List to hold references to cloned objectives for this quest
    // These are the instances of objectives that can be completed
    public List<ObjectiveSO> clonedObjectives = new List<ObjectiveSO>();

    //Try to complete quest

    public void TryEndQuest() {
        // Check if quest has no objectives set to it
        if (clonedObjectives == null || clonedObjectives.Count == 0) {
            Debug.LogError("No objectives found for the quest.");
            return;
        }

        // Check if all required objectives are completed

        // Start with true value
        bool allRequiredObjectivesCompleted = true;
        // Loop through objectives
        foreach (var objective in clonedObjectives) {
            Debug.Log("Objectives : " + objective.description + " status : " + objective.Completed);
            // If iterated objective in the loop is required and is not completed set requirement value as false and break off from loop
            // !!!IMPROVE LOGIC HERE!!!! Would like to check through every objective and see which ones are completed and which ones are not, and Debug.Log them at the end of the function
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
    void InitializeQuest() {
        // Example: Add objectives to the quest
        // You can call AddObjective() multiple times to add different objectives
        // Example:
        // AddObjective(objective1);
        // AddObjective(objective2);
        if (clonedObjectives.Count == 0) { 
            foreach (var objective in objectives) {
                AddObjective(objective);
            }
        }
        
    }
    // Initializes each ObjectiveSO in the objectives list by setting its parentQuest property to reference this QuestSO instance.
    void OnEnable() {
        // Call a method here to add objectives to the quest dynamically
        InitializeQuest();
    }
    void OnDisable() {
        clonedObjectives.Clear();
    }
    // Method to dynamically add an objective to the quest
    public void AddObjective( ObjectiveSO objective ) {
        // Clone the ObjectiveSO before adding it to the quest
        ObjectiveSO clonedObjective = objective.Clone();
        // Assign the parent quest to the cloned objective
        clonedObjective.parentQuest = this;
        // Add the cloned objective to the quest's list of objectives
        clonedObjectives.Add(clonedObjective);
        Debug.Log("Cloned objective added " + clonedObjective.description + " to " + clonedObjective.parentQuest + " quest");
    }
}
