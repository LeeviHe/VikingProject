using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour {
    // The objective associated with this quest item
    // can hide but keep visible for debugging
    public ObjectiveSO objective;
    // Set the objective for this quest item
    public void SetObjective( ObjectiveSO newObjective ) {
        // Assign the new objective
        objective = newObjective;
        // Mark the new objective as incomplete
        newObjective.Completed = false;
        Debug.Log("State : " + newObjective.Completed);
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
