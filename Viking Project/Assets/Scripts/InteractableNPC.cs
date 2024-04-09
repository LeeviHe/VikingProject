using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : MonoBehaviour, IInteractable {
    [SerializeField] private string interactText;
    public void Interact(Player player) {
        Debug.Log("Hello there!");
    }

    public string GetInteractText() { 
        return interactText;
    }
    public Transform GetTransform() {
        return transform;
    }
}
