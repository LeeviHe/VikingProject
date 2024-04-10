using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WeaponSO : ScriptableObject {
    public Transform prefab;
    public string objectName;
    public int minDamage;
    public int maxDamage;
    public float attackSpeed;
    public float hitRange;
    public float blockingPower;
}