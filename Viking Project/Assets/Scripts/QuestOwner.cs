using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestOwner : MonoBehaviour {
    // The quest associated with this quest owner
    public QuestSO myQuest;

    // List of quest items owned by this quest owner
    public List<QuestItem> questItem = new List<QuestItem>();

    // Give the quest to the player
    public void GiveQuest( Player player ) {
        // Pass the quest to the player to start tracking
        player.ReceiveNewQuest(myQuest);

        // Loop through the quest items
        for (int i = 0; i < questItem.Count; i++) {
            // Check if the current quest item has a corresponding objective in the quest
            if (i < myQuest.clonedObjectives.Count) {
                // Set the objective for the quest item
                questItem[i].SetObjective(myQuest.clonedObjectives[i]);
                Debug.Log("Add cloned objective " + myQuest.clonedObjectives[i] + " to " + questItem[i]);
            } else {
                // Log a warning if there are not enough objectives in the quest for all quest items
                Debug.LogWarning("Not enough objectives in the quest for all quest items.");
                break;
            }
        }
    }
}