using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour {
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;

    private void Update() {
        // Check if there is an interactable object within range
        if (playerInteract.GetInteractableObject() != null) {
            // If so, display the interact UI
            Show(playerInteract.GetInteractableObject());
        } else {
            // Otherwise, hide the interact UI
            Hide();
        }
    }

    // Show the interact UI with the provided interactable object
    private void Show( IInteractable interactable ) {
        // Activate the UI container GameObject
        containerGameObject.SetActive(true);
        // Set the text of the TextMeshProUGUI component to the interact text of the provided interactable object
        interactTextMeshProUGUI.text = interactable.GetInteractText();
    }

    // Hide the interact UI
    private void Hide() {
        // Deactivate the UI container GameObject
        containerGameObject.SetActive(false);
    }
}