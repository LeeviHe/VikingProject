using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public WeaponSO weaponSO; // The SO representing this weapon's values
    private IWeaponParent weaponParent; // Reference to the parent object (e.g., player's hand)

    [HideInInspector] public int minDamage;
    [HideInInspector] public int maxDamage;
    [HideInInspector] public float hitRange;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float blockingPower;
    [HideInInspector] public bool twoHanded;

    //Set weaponSO values to the weapon
    private void Awake() {
        minDamage = weaponSO.minDamage;
        maxDamage = weaponSO.maxDamage;
        hitRange = weaponSO.hitRange;
        attackSpeed = weaponSO.attackSpeed;
        blockingPower = weaponSO.blockingPower;
        twoHanded = weaponSO.twoHanded;
    }

    //!!!POLISH!!! Questionable functionality
    //Set Weapon to a parent object (players hand)
    public void SetWeaponParent( IWeaponParent weaponParent ) {
        //Set the new parent (hand) for the weapon
        this.weaponParent = weaponParent;
        weaponParent.SetWeapon(weaponSO, gameObject);
        transform.parent = weaponParent.GetWeaponFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    // Destroy the weapon object
    public void DestroySelf() {
        // Clear the weapon from the parent
        weaponParent.ClearWeapon();
        // Destroy the weapon game object
        Destroy(weaponSO.prefab.transform);
    }

    // Spawn a new weapon based on the provided WeaponSO and parent
    public static Weapon SpawnWeapon( WeaponSO weaponSO, IWeaponParent weaponParent, Quaternion rotation) {
        // Instantiate the weapon prefab
        Transform weaponTransform = Instantiate(weaponSO.prefab.transform);
        // Get the Weapon component from the instantiated object
        Weapon weapon = weaponTransform.GetComponent<Weapon>();
        // Set the weapon's parent
        weapon.SetWeaponParent(weaponParent);
        weaponTransform.localRotation = rotation;
        return weapon;
    }
}
