using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponParent {
    public Transform GetWeaponFollowTransform();

    public void SetWeapon( Weapon weapon );

    public Weapon GetWeapon();

    public void ClearWeapon();

    public bool HasWeapon(); 
}
