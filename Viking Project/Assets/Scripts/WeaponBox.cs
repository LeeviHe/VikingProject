using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : QuestItem, IWeaponParent, IInteractable {

    [SerializeField] private Transform boxHoldingPoint; // The transform where the weapon is set on the object
    [SerializeField] private WeaponSO weaponSO; // The ScriptableObject representing the weapon properties

    private Weapon weapon; //Reference to the spawned weapon

    //Spawn the weapon on the boxholdingpoint on initialize
    private void Start() {
        Weapon.SpawnWeapon(weaponSO, this);
    }

    private void InteractWeaponBox( Player player ) {
        //Interact/complete the objective if there is any
        if (objective) { 
            ObjectiveInteraction();
        }
        //Check if weaponBox has a weapon
        if (HasWeapon()) {
            //Player is carrying weapon
            if (player.HasWeapon()) {
                //Clear old weapon
                player.GetWeapon().DestroySelf();
            }
        // Spawn weapon on player
        Weapon.SpawnWeapon(weaponSO, player);
        }
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
