using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingStone : MonoBehaviour, IInteractable {
    public List<BlessingSO> blessings = new List<BlessingSO>();
    public UIElementManager uiElementManager;
    private void InteractWithBlessingStone() {
        uiElementManager.ToggleBlessingScreen();
    }

    public string GetInteractText() {
        return "Choose blessing";
    }

    public Transform GetTransform() {
        return transform;
    }

    public void Interact( PlayerController player ) {
        InteractWithBlessingStone();
    }
}
