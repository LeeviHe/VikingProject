using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    // Consider if Player player parameter is useful or an issue
    void Interact(Player player);
    string GetInteractText();

    Transform GetTransform();
}
