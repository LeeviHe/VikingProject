using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : QuestBoard, IInteractable {
    [SerializeField] private string interactText;

    //Handle interaction with NPC
    public void Interact( PlayerController player ) {
        //Check if player doesn't have a quest and that the NPC has a quest to give
        if (!player.currentQuest && myQuest != null) {
            //Give quest
            //!!!POLISH!!! Maybe clear NPC myQuest, so the NPC cannot give the same quest multiple times
            Debug.Log("Quest Given");
            GiveQuestToPlayer(player);
        }
        //Interaction events here
        Debug.Log("Hello there!");
    }

    public string GetInteractText() { 
        return interactText;
    }
    public Transform GetTransform() {
        return transform;
    }
}
