using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private WeaponSO weaponSO;
    private IWeaponParent weaponParent;
    [HideInInspector] public int minDamage;
    [HideInInspector] public int maxDamage;
    [HideInInspector] public float hitRange;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float blockingPower;
    private void Awake() {
        minDamage = weaponSO.minDamage;
        maxDamage = weaponSO.maxDamage;
        hitRange = weaponSO.hitRange;
        attackSpeed = weaponSO.attackSpeed;
        blockingPower = weaponSO.blockingPower;
    }
    public WeaponSO GetWeaponSO() { 
        return weaponSO;
    }
    public void SetWeaponParent( IWeaponParent weaponParent ) {
        // Would clear weapon from table (old parent)
        if (this.weaponParent != null) {
            this.weaponParent.ClearWeapon();
        }

        this.weaponParent = weaponParent;
        if (weaponParent.HasWeapon()) {
            Debug.LogError("IWeaponParent already has a WeaponParent");
        } else { 
            weaponParent.SetWeapon(this);
            transform.parent = weaponParent.GetWeaponFollowTransform();
            transform.localPosition = Vector3.zero;
        }
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
