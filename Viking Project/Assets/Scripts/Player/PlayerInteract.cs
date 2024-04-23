using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour {
    [SerializeField] private GameObject playerObject;

    public void HandleInteract() {
        IInteractable interactable = GetInteractableObject();
        // If there is an interactable object
        if (interactable != null) {
            // Call the Interact method of the interactable object, passing the player object
            interactable.Interact(playerObject.GetComponent<Player>());
        }
    }
    // Find and return the interactable object within range
    public IInteractable GetInteractableObject() {
        // List to store all nearby interactable objects
        List<IInteractable> interactableList = new List<IInteractable>();
        // Define the range within which to search for interactable objects
        float interactRange = 2f;
        // Find all colliders within the specified range
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        // Iterate through each collider found within the range
        foreach (Collider collider in colliderArray) {
            // Check if the collider has an IInteractable component
            if (collider.TryGetComponent(out IInteractable interactable)) {
                // Add the interactable object to the list
                interactableList.Add(interactable);
            }
        }
        // Determine the closest interactable object to the player
        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList) {
            // If no closest interactable is set yet, set it to the current one
            if (closestInteractable == null) {
                closestInteractable = interactable;
            } else {
                // If the distance to the current interactable is shorter than the previous closest one, set it as the new closest one
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) {
                    closestInteractable = interactable;
                }
            }
        }
        // Return the closest interactable object
        return closestInteractable;
    }
}