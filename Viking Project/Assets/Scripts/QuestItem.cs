using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour {
    public ObjectiveSO objective;
    public void SetObjective( ObjectiveSO newObjective ) {
        objective = newObjective;
        newObjective.Completed = false;
        if (objective != null && objective.waypoint != null) {
            objective.waypoint = transform;
        }
    }

    public void ObjectiveInteraction() {
        if (objective != null) {
            objective.CompleteObjective();
            objective = null;
        } else {
            //Not an objective/
        }
    }
}
