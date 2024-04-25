using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour, IInteractable {
    [SerializeField] private List<Weapon> weaponsList = new List<Weapon>();
    private void InteractWeaponBox( PlayerController player ) {
        //Interact/complete the objective if there is any
        Debug.Log("Available weapons in the box:");
        for (int i = 0; i < weaponsList.Count; i++) {
            Debug.Log((i + 1) + ". " + weaponsList[i].weaponSO.objectName);
        }
        int choice = GetPlayerChoice();
        if (choice > 0 && choice <= weaponsList.Count) {
            Weapon chosenSlot = weaponsList[choice - 1];
            if (player.HasWeapon()) {
                //Clear old weapon
                player.GetWeapon().DestroySelf();
            }
            // Set player's weapon as the chosen weapon
            player.SetWeapon(chosenSlot);
            Debug.Log("player took " + chosenSlot);
            PlayerData.Instance.UpdateWeapon(chosenSlot);
            Weapon.SpawnWeapon(chosenSlot.weaponSO, player, chosenSlot.weaponSO.prefab.transform.rotation);
        }
    }

    private int GetPlayerChoice() {
        // Implement a method to get player's choice, e.g., using input
        // For simplicity, return 1 for now
        return 1;
    }
    public string GetInteractText() {
        return "Pick up weapon";
    }

    public Transform GetTransform() {
        return transform;
    }

    public void Interact(PlayerController player) {
        InteractWeaponBox(player);
    }
}
