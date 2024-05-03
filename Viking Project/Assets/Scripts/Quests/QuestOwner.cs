using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestOwner : MonoBehaviour {
    // The quest associated with this quest owner
    public QuestSO myQuest;

    // List of pre-assigned quest items for this NPC's quest
    public List<QuestItem> questItems = new List<QuestItem>();

    // Give the quest to the player and initialize quest items
    public void GiveQuestToPlayer( PlayerController player ) {
        // Pass the quest to the player to start tracking
        player.ReceiveNewQuest(myQuest);
    }
}