using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private WeaponSO weaponSO; // The SO representing this weapon's values
    private IWeaponParent weaponParent; // Reference to the parent object (e.g., player's hand)
    [HideInInspector] public int minDamage;
    [HideInInspector] public int maxDamage;
    [HideInInspector] public float hitRange;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float blockingPower;

    //Set weaponSO values to the weapon
    private void Awake() {
        minDamage = weaponSO.minDamage;
        maxDamage = weaponSO.maxDamage;
        hitRange = weaponSO.hitRange;
        attackSpeed = weaponSO.attackSpeed;
        blockingPower = weaponSO.blockingPower;
    }
    // Get the WeaponSO associated with this weapon
    public WeaponSO GetWeaponSO() { 
        return weaponSO;
    }
    //!!!POLISH!!! Questionable functionality
    //Set Weapon to a parent object (players hand)
    public void SetWeaponParent( IWeaponParent weaponParent ) {
        // Clear the weapon from the previous parent (if any)
        if (this.weaponParent != null) {
            this.weaponParent.ClearWeapon();
        }
        //Set the new parent (hand) for the weapon
        this.weaponParent = weaponParent;

        // If parent doesn't have a weapon, attach the weapon to the new parent's follow transform
        if (!weaponParent.HasWeapon()) {
            weaponParent.SetWeapon(this);
            transform.parent = weaponParent.GetWeaponFollowTransform();
            transform.localPosition = Vector3.zero;
        } else {
            // Log an error if the parent already has a weapon
            Debug.Log("IWeaponParent already has a WeaponParent");
        }
    }

    // Get the parent object of the weapon
    public IWeaponParent GetWeaponParent() {
        return weaponParent;
    }

    // Destroy the weapon object
    public void DestroySelf() {
        // Clear the weapon from the parent
        weaponParent.ClearWeapon();
        // Destroy the weapon game object
        Destroy(gameObject);
    }

    // Spawn a new weapon based on the provided WeaponSO and parent
    public static Weapon SpawnWeapon( WeaponSO weaponSO, IWeaponParent weaponParent ) {
        // Instantiate the weapon prefab
        Transform weaponTransform = Instantiate(weaponSO.prefab);
        // Get the Weapon component from the instantiated object
        Weapon weapon = weaponTransform.GetComponent<Weapon>();
        // Set the weapon's parent
        weapon.SetWeaponParent(weaponParent);
        return weapon;
    }
}
