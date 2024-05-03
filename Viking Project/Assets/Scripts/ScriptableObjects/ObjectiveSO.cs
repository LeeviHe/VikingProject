using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectiveSO : ScriptableObject {
    [HideInInspector] public QuestSO parentQuest;
    public bool required = true;
    public bool Completed { get; set; }
    public Transform waypoint;
    public GameObject objectiveItemPrefab;
    [TextArea]
    public string description;

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
}