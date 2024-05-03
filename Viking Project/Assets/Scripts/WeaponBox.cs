using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour, IInteractable {
    [SerializeField] private List<Weapon> weaponsList = new List<Weapon>();
    public UIElementManager uiElementManager;
    public PlayerController playerController;

    private void InteractWeaponBox() {
        uiElementManager.ToggleScreen(uiElementManager.weaponSelectorUI);
    }

    public void EquipWeapon( int weaponIteration ) {
        if (!playerController) {
            Debug.Log("Issue with playerController detection");
        } else { 
            if (playerController.HasWeapon()) {
                playerController.GetWeapon().DestroySelf();
            }
            playerController.SetWeapon(weaponsList[weaponIteration]);
            Debug.Log("player took " + weaponsList[weaponIteration]);
            PlayerData.Instance.UpdateWeapon(weaponsList[weaponIteration]);
            Weapon.SpawnWeapon(weaponsList[weaponIteration].weaponSO, playerController, weaponsList[weaponIteration].weaponSO.prefab.transform.rotation);
            uiElementManager.ToggleScreen(uiElementManager.weaponSelectorUI);
            Time.timeScale = 1f;
        }
    }

    public string GetInteractText() {
        return "Pick up weapon";
    }

    public Transform GetTransform() {
        return transform;
    }

    public void Interact(PlayerController player) {
        InteractWeaponBox();
    }
}
