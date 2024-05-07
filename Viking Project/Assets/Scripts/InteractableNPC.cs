using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : QuestBoard, IInteractable {
    [SerializeField] private string interactText;

    //Handle interaction with NPC
    public void Interact( PlayerController player ) {
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
