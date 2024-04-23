using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestOwner : MonoBehaviour {
    // The quest associated with this quest owner
    public QuestSO myQuest;

    // List of pre-assigned quest items for this NPC's quest
    public List<QuestItem> questItems = new List<QuestItem>();
    public GameObject questItemPrefab;
    public Transform spawnPoint;

    // Give the quest to the player and initialize quest items
    public void GiveQuestToPlayer( PlayerController player ) {
        // Pass the quest to the player to start tracking
        player.ReceiveNewQuest(myQuest);
        // Initialize quest items if they haven't been initialized yet
        questItems.Clear();
        // Initialize quest items
        foreach (ObjectiveSO objective in myQuest.objectives) {
            // Instantiate a new GameObject for each quest objective
            GameObject questItemGO = Instantiate(questItemPrefab, spawnPoint.position, Quaternion.identity);
            questItemGO.transform.SetParent(transform);
            // Get the QuestItem component attached to the newly instantiated GameObject
            QuestItem questItem = questItemGO.GetComponent<QuestItem>();
            // Set the objective for the QuestItem
            questItem.SetObjective(objective);
            // Add the QuestItem to the list of quest items owned by this quest owner
            questItems.Add(questItem);
            Debug.Log("Add quest item " + questItemGO.name + " to NPC's quest");
        }
    }
}