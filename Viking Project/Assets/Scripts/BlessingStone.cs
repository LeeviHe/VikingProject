/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingStone : MonoBehaviour, IInteractable {
    private Blessing blessing;
    [SerializeField] private BlessingSO blessingSO;
    private void InteractWithBlessing(PlayerController player) {
        Debug.Log(blessingSO + " equipped");
        blessing.EquipBlessing(blessingSO, player);
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
}*/
