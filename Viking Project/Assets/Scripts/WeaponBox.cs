using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBox : MonoBehaviour, IInteractable {
    [SerializeField] private List<WeaponSO> weaponsList = new List<WeaponSO>();
    public UIElementManager uiElementManager;
    public PlayerController playerController;

    private void InteractWeaponBox() {
        uiElementManager.ToggleScreen(uiElementManager.weaponSelectorUI);
        if (!playerController.canWieldTwoHanded) {
            uiElementManager.axeImage.color = Color.grey;
            uiElementManager.axeButton.GetComponent<Button>().interactable = false;
            uiElementManager.axeButtonText.text = "Berserker blessing required";
        } else {
            uiElementManager.axeImage.color = Color.white;
            uiElementManager.axeButton.GetComponent<Button>().interactable = true;
            uiElementManager.axeButtonText.text = "Choose";
        }
    }

    public void EquipWeapon( int weaponIteration ) {
        if (playerController.HasWeapon()) {
            playerController.ClearWeapon();
        }
        if (weaponsList[weaponIteration].twoHanded) {
            Debug.Log("Weapon is two-handed");
            if (playerController.canWieldTwoHanded) {
                Debug.Log("Player can wield two-handed");
                Debug.Log("player took " + weaponsList[weaponIteration]);
                PlayerData.Instance.UpdateWeapon(weaponsList[weaponIteration]);
                Weapon.SpawnWeapon(weaponsList[weaponIteration], playerController, weaponsList[weaponIteration].prefab.transform.rotation, playerController);
                uiElementManager.ToggleScreen(uiElementManager.weaponSelectorUI);
                Time.timeScale = 1f;
            } else {
                Debug.Log("Player cannot wield two-handed");
            }
        } else {
            Debug.Log("Weapon is not two-handed");
            Debug.Log("player took " + weaponsList[weaponIteration]);
            PlayerData.Instance.UpdateWeapon(weaponsList[weaponIteration]);
            Weapon.SpawnWeapon(weaponsList[weaponIteration], playerController, weaponsList[weaponIteration].prefab.transform.rotation, playerController);
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

    public void Interact( PlayerController player ) {
        InteractWeaponBox();
    }
}
