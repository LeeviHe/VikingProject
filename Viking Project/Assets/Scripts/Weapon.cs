using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private WeaponSO weaponSO;
    private IWeaponParent weaponParent;
    public int minDamage;
    public int maxDamage;
    public float hitRange;
    public float attackSpeed;
    private void Awake() {
        minDamage = weaponSO.minDamage;
        maxDamage = weaponSO.maxDamage;
        hitRange = weaponSO.hitRange;
        attackSpeed = weaponSO.attackSpeed;
    }
    public WeaponSO GetWeaponSO() { 
        return weaponSO;
    }
    public void SetWeaponParent( IWeaponParent weaponParent ) {
        if (this.weaponParent != null) {
            this.weaponParent.ClearWeapon();
        }

        this.weaponParent = weaponParent;

        if (weaponParent.HasWeapon()) {
            Debug.LogError("IWeaponParent already has a WeaponParent");
        }
        weaponParent.SetWeapon(this);
        transform.parent = weaponParent.GetWeaponFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IWeaponParent GetWeaponParent() {
        return weaponParent;
    }

    public void DestroySelf() {
        weaponParent.ClearWeapon();
        Destroy(gameObject);
    }

    public static Weapon SpawnWeapon( WeaponSO weaponSO, IWeaponParent weaponParent) {
        Transform weaponTransform = Instantiate(weaponSO.prefab);

        Weapon weapon = weaponTransform.GetComponent<Weapon>();

        weapon.SetWeaponParent(weaponParent);

        return weapon;
    }
}