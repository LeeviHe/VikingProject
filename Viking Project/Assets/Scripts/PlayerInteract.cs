using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
    [SerializeField] private GameObject playerObject;
    public LayerMask interactableLayer;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            IInteractable interactable = GetInteractableObject();
            if (interactable != null) { 
                interactable.Interact(playerObject.GetComponent<Player>());
            }
        }
    }


    public IInteractable GetInteractableObject() {
        List<IInteractable> interactableList = new List<IInteractable>();
        float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);

        foreach (Collider collider in colliderArray) {
            if (collider.TryGetComponent(out IInteractable interactable)) {
                interactableList.Add(interactable);
            }
        }

        //Getting the closest npc so the UI doesn't freak out
        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList) {
            // If no closest npc, set it as the next one
            if (closestInteractable == null) {
                closestInteractable = interactable;
            } else {
                //If the distance to an NPC is shorter than the previous closest one, set it as the new closest NPC
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) {
                    // 
                    closestInteractable = interactable;
                }
            }
        }
        return closestInteractable;
    }
}
/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
    public LayerMask interactableLayer;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) { 
            float interactRange = 2f;
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange, interactableLayer);

            foreach (Collider collider in colliders) {
                IInteractable interactable = collider.GetComponent<IInteractable>();
                if (interactable != null) 
                {
                    if (collider.TryGetComponent(out InteractableNPC interactableNPC)) {
                        interactableNPC.Interact();
                    }
                }
            }
        }
    }
}

 */