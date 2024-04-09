using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteractable {
    private void InteractWithItem() {
        Debug.Log("Interacted with item");
    }
    public string GetInteractText() {
        return "Interact with item";
    }

    public void Interact( Player player ) {
        InteractWithItem();
    }

    public Transform GetTransform() { 
        return transform; 
    }
}
