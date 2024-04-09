using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectiveSO : ScriptableObject {
    [HideInInspector] public QuestSO parentQuest;
    public bool required = true;
    public bool Completed{ get; set; }
    public Transform waypoint;
    [TextArea]
    public string description;

    public void CompleteObjective () {
        Debug.Log("Objective completed");
        Completed = true;
        if (parentQuest != null) {
            parentQuest.TryEndQuest();
        }
    }

    public void AssignParentQuest( QuestSO quest ) { 
        parentQuest = quest;
    }
}
