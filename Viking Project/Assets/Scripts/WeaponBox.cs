using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour, IWeaponParent, IInteractable {

    [SerializeField] private Transform boxHoldingPoint;
    [SerializeField] private WeaponSO weaponSO;

    private Weapon weapon;
    private void InteractWeaponBox( Player player ) {
        Debug.Log("Interacted with weapon box");
        if (!player.HasWeapon()) {
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
