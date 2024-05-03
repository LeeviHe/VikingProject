using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingStone : MonoBehaviour, IInteractable {
    public List<BlessingSO> blessings = new List<BlessingSO>();
    public UIElementManager uiElementManager;
    public PlayerController playerController;

    private void InteractWithBlessingStone() {
        uiElementManager.ToggleScreen(uiElementManager.blessingSelectorUI);
    }

    public void EquipBlessing( int blessingIteration ) {
        // Remove effects of previously equipped blessing (if any)
        playerController.RemoveBlessingEffects();
        Debug.Log(blessings[blessingIteration]);
        playerController.currentBlessing = blessings[blessingIteration];
        // Apply new blessing effects
        blessings[blessingIteration].ApplyBlessing(playerController);
        PlayerData.Instance.UpdateBlessing(blessings[blessingIteration]);
        uiElementManager.ToggleScreen(uiElementManager.blessingSelectorUI);
        Time.timeScale = 1f;   
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
