using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : QuestItem, IWeaponParent, IInteractable {

    [SerializeField] private Transform boxHoldingPoint;
    [SerializeField] private WeaponSO weaponSO;

    private Weapon weapon;

    private void Start() {
        Weapon.SpawnWeapon(weaponSO, this);
    }

    private void InteractWeaponBox( Player player ) {
        ObjectiveInteraction();
        if (HasWeapon()) {
            //Player is carrying weapon
            if (player.HasWeapon()) {
                //Clear old weapon
                player.GetWeapon().DestroySelf();
            }
        // Spawn weapon on player
        Weapon.SpawnWeapon(weaponSO, player);
        }
        Debug.Log("Spawned weapon: " + weaponSO);
    }

    public void ClearWeapon() {
        weapon = null;
    }

    public string GetInteractText() {
        return "Pick up weapon";
    }

    public Transform GetTransform() {
        return transform;
    }

    public Weapon GetWeapon() {
        return weapon;
    }

    public Transform GetWeaponFollowTransform() {
        return boxHoldingPoint;
    }

    public bool HasWeapon() {
        return weapon != null;
    }

    public void Interact(Player player) {
        InteractWeaponBox(player);
    }

    public void SetWeapon( Weapon weapon ) {
        this.weapon = weapon;
    }
}
