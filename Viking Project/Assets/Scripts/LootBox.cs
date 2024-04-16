using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootBox : QuestItem, IInteractable {
    [SerializeField] private Transform itemHoldingPoint;
    [SerializeField] private GameObject loot;
    GameObject lootItem;

    public void Start() {
        lootItem = Instantiate(loot, itemHoldingPoint);
    }

    public void InteractLootBox() {
        if (loot) {
            Debug.Log(loot + " collected");
            loot = null;
            Destroy(lootItem);
        } else {
            Debug.Log("Lootbox is empty");
        }
        if (objective) {
            ObjectiveInteraction();
        }
    }

    public void Interact( Player player ) {
        InteractLootBox();
    }

    public string GetInteractText() {
        if (loot) {
            return "Take loot";
        } else {
            return "Box is empty";
        }
        
    }

    public Transform GetTransform() {
        return transform;
    }
}
