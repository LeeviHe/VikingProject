using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : QuestOwner, IInteractable {
    [SerializeField] private string interactText;

    public void Interact(Player player) {
        if (!player.currentQuest && myQuest != null) {
            Debug.Log("Quest Given");
            GiveQuest(player);
        }
        
        Debug.Log("Hello there!");
    }

    public string GetInteractText() { 
        return interactText;
    }
    public Transform GetTransform() {
        return transform;
    }
}
