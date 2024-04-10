using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NPCStatsSO : ScriptableObject {
    public float maxHealth;
    public float armor;
    public float damage;
    public float movementSpeed;
    public float attackCooldown;
    public float attackRange;
    public float aggroRange;
}
