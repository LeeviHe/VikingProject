using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestOwner : MonoBehaviour {
    public QuestSO myQuest;
    public List<QuestItem> questItem = new List<QuestItem>();

    public void GiveQuest( Player player ) {
        player.ReceiveNewQuest(myQuest);
        for (int i = 0; i < questItem.Count; i++) {
            if (i < myQuest.objectives.Count) {
                questItem[i].SetObjective(myQuest.objectives[i]);
            } else {
                Debug.LogWarning("Not enough objectives in the quest for all quest items.");
                break;
            }
        }
    }
}
