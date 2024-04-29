﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

//Fireball spell
[CreateAssetMenu(fileName = "New Fireball Spell", menuName = "Abilities/Fireball Spell")]
public class FireballSO : SpellSO {
    public int damage;
    public int burnDamage;
    public int burnTime;
    public float radius;
    public LayerMask enemyLayer; // Layer mask for enemies

    public void Explosion( Transform origin ) {
        Debug.Log("Boom");
        List<Collider> allHitEnemies = new List<Collider>();

        // Set aiming in playerCombat
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
        Explosion(origin);
    }
}




















/*
public class Blessing : MonoBehaviour {

    public void EquipBlessing( BlessingSO blessingSO, PlayerController player ) {
        // Remove effects of previously equipped blessing (if any)
        RemoveBlessingEffects(player);

        // Apply new blessing effects
        blessingSO.ApplyBlessing(player);
        PlayerData.Instance.UpdateBlessing(blessingSO);
    }
    public void EquipBlessing( BlessingSO blessingSO ) {
        // Remove effects of previously equipped blessing (if any)
        RemoveBlessingEffects();
        // Apply new blessing effects
        blessingSO.ApplyBlessing(this);
        PlayerData.Instance.UpdateBlessing(blessingSO);
    }
    public void ModifyStat( float stat, float modifier ) {
        stat += modifier;
    }
    private void RemoveBlessingEffects( PlayerController player ) {
        // Reset stats to base values
        player.currentMoveSpeed = player.stats.movementSpeed;
        player.currentHealth = player.stats.maxHealth;
    }
}*/
