using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour, IInteractable {
    // The quest associated with this quest owner
    public QuestSO myQuest;
    public UIElementManager elementManager;


    public void InteractWithQuestBoard() {
        elementManager.ToggleScreen(elementManager.questBoardUI);
    }

    // Give the quest to the player and initialize quest items
    public void GiveQuestToPlayer( PlayerController player ) {
        // Pass the quest to the player to start tracking
        player.ReceiveNewQuest(myQuest);
    }

    public void Interact( PlayerController player ) {
        InteractWithQuestBoard();
    }    
    public string GetInteractText() {
        return "Inspect Quest Board";
    }

    public Transform GetTransform() {
        return transform;
    }

}