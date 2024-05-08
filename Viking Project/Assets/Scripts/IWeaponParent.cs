using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponParent {
    public Transform GetWeaponFollowTransform();

    public void SetWeapon( WeaponSO weapon, GameObject weaponObject);

    public GameObject GetWeapon();

    public void ClearWeapon();

    public bool HasWeapon(); 
}
