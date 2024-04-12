using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NPCStatsSO : ScriptableObject {
    public float maxHealth;
    public int armor;
    public int minDamage;
    public int maxDamage;
    public float movementSpeed;
    public float attackCooldown;
    public float attackRange;
    public float aggroRange;
}
