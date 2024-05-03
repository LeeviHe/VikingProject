using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour {
    // The objective associated with this quest item
    // can hide but keep visible for debugging
    public QuestSO parentQuest;
    public ObjectiveSO objective;
    // Set the objective for this quest item
    public void SetObjective( ObjectiveSO newObjective, QuestSO parentQuest ) {
        // Assign the new objective
        objective = newObjective;

        // Set the parent quest of the objective to the quest associated with this quest item's owner (if available)
        if (objective != null) {
            Debug.Log(parentQuest);
            objective.parentQuest = parentQuest;
            this.parentQuest = parentQuest;
        }

        // Mark the new objective as incomplete
        if (objective != null) {
            objective.Completed = false;
        }

        // If the objective and its waypoint are defined, assign the waypoint to this object's transform
        if (objective != null && objective.waypoint != null) {
            objective.waypoint = transform;
        }
    }

    // Perform interaction related to the objective
    public void ObjectiveInteraction() {
        // Check if an objective is associated with this quest item
        if (objective != null) {
            // Complete the associated objective
            objective.CompleteObjective();
            // Clear the objective reference
            objective = null;
        } else {
            // If there's no objective associated, perform other interactions
            // Not an objective
        }
    }
}