using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

//Fireball spell
[CreateAssetMenu(fileName = "New Fireball Spell", menuName = "Abilities/Fireball Spell")]
public class FireballSO : SpellSO {
    public int damage;
    public int burnDamage;
    public int burnTime;
    public float radius;
    public float acceleration;
    public float rotationSpeed;
    public LayerMask enemyLayer; // Layer mask for enemies

    public void ExecuteSpell( Transform origin ) {
        Vector3 spawnPosition = origin.position + Vector3.up * 3;
        GameObject spell = Instantiate(spellPrefab, spawnPosition, origin.rotation);
        spell.transform.parent = origin;
        Destroy(spell, spellLife);
    }

    public void Explosion( Transform origin ) {  
        List<Collider> allHitEnemies = new List<Collider>();
        var effect = Instantiate(spellEffectPrefab, origin.transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Collider[] potentialHitEnemies = Physics.OverlapSphere(origin.position, radius, enemyLayer);
        foreach (Collider enemy in potentialHitEnemies) {
            if (!allHitEnemies.Contains(enemy)) {
                // Apply damage to the enemy
                if (enemy.GetComponent<EnemyNpc>() != null) {
                    // Damage dealt is randomly set on each hit, defined between weapon's damage stats
                    int damage = this.damage;
                    Debug.Log("Applied " + damage + " to " + enemy);
                    enemy.GetComponent<EnemyNpc>().TakeDamage(damage);
                    allHitEnemies.Add(enemy);
                    ApplyBurnEffect(enemy);
                    Debug.Log("Applied burn to " + enemy);
                }
            }
        }
    }


    public void ApplyBurnEffect(Collider enemy) {
        //Add status effect to enemy for a set time
        EnemyNpc enemyNPC = enemy.GetComponent<EnemyNpc>();
        enemyNPC.activeSpell = this;
    }

    public IEnumerator Burn( EnemyNpc target ) {
        // Coroutine to perform burn to target
        int timeBurned = burnTime;
        Debug.Log(target);
        Debug.Log(burnDamage);
        while (timeBurned > 0) {
            yield return new WaitForSeconds(2);
            target.TakeDamage(burnDamage);
            timeBurned--;
            yield return null;
        }
        target.activeSpell = null;
    }
    protected override void ExecuteAbility( Transform origin ) {
        // Execute the functionality of the Fireball spell
        ExecuteSpell(origin);
    }
}