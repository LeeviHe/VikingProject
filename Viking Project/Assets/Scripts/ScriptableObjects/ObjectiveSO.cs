using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectiveSO : ScriptableObject {
    [HideInInspector] public QuestSO parentQuest;
    public bool required = true;
    public bool Completed { get; set; }
    public Transform waypoint;
    [TextArea]
    public string description;

    // Counter to keep track of the iteration count
    private static int cloneCounter = 1;
    void OnEnable() {
        cloneCounter = 1;
    }
    //Function for completing a single objective

    public void CompleteObjective() {
        Debug.Log("Objective completed");
        //Set Completed value of objective to true 
        Completed = true;
        // If objective is set to a quest
        if (parentQuest != null) {
            //Perform quest completion check in QuestSO
            parentQuest.TryEndQuest();
        } else {
            Debug.LogError("Objective not set to a quest");
        }
    }
    public ObjectiveSO Clone() {
        // Create a new instance of ObjectiveSO
        ObjectiveSO clone = ScriptableObject.CreateInstance<ObjectiveSO>();
        // Copy properties from the original ObjectiveSO to the clone
        clone.required = required;
        clone.waypoint = waypoint;
        // Append the iteration count to the description of the clone
        clone.description = description + " (Clone " + cloneCounter + ")";
        // Increment the clone counter for the next clone
        cloneCounter++;
        return clone;
    }
}