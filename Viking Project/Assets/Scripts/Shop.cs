using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractable {

    public SelectorUIManager uiElementManager;
    public PlayerController playerController;

    public void Update() {
        uiElementManager.potionCountText.text = playerController.healthPotions + "/" + playerController.maxHealthPotions;
    }

    public void OpenShop() {
        uiElementManager.ToggleScreen(uiElementManager.shopScreen);
    }

    public void PurchaseItem() {
        int potionPrice = 5;
        if (playerController.healthPotions < playerController.maxHealthPotions) {
            if (PlayerData.Instance.coins >= potionPrice) {
                Debug.Log("Potion purchased");
                PlayerData.Instance.coins -= potionPrice;
                PlayerData.Instance.healthPotions++;
            } else {
                Debug.Log("Not enough coins");
            }
        } else {
            Debug.Log("Player has max potions");
        }
    }

    public string GetInteractText() {
       return "Open shop";
    }

    public Transform GetTransform() {
        return transform;
    }

    public void Interact( PlayerController player ) {
        OpenShop();
    }
}
