using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingStone : MonoBehaviour, IInteractable {
    [SerializeField] private BlessingSO blessingSO;
    private void InteractWithBlessing(PlayerController player) {
        player.EquipBlessing(blessingSO);
    }

    public string GetInteractText() {
        return "Equip " + blessingSO.blessingName + " blessing";
    }

    public Transform GetTransform() {
        return transform;
    }

    public void Interact( PlayerController player ) {
        InteractWithBlessing(player);
    }
}
