using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    [SerializeField] private PlayerController playerController;

    [Header("Health Stats")]
    private float lastDamageTime; // Time when player last took damage
    private float damageCooldown = 2f; // Cooldown duration in seconds
    [Header("Weapon fields")]
    public WeaponSO weapon; //Weapon of player
    public GameObject weaponObject;
    public float attackDuration;
    [Header("Enemy layer")]
    public LayerMask enemyLayer; // Define the layer for enemy NPCs

    public static event Action OnPlayerDeath;

    private void LateUpdate() {
        weapon = playerController.weapon;
        if (weapon) {
            weaponObject = weapon.prefab;
        }
    }

    public IEnumerator PerformAttack() {
        playerController.playerAnimations.playerAnimator.speed = 1.0f / attackDuration;
        yield return new WaitForSeconds(attackDuration * 0.8f);
        // Track enemies hit during the entire attack animation
        List<Collider> allHitEnemies = new List<Collider>();
        Debug.Log("start detection");
        while (attackDuration > 0) {
            playerController.playerAnimations.playerAnimator.SetBool("Block", false);
            playerController.isBlocking = false;
            // Detect enemies in attack range
            Collider[] potentialHitEnemies = Physics.OverlapSphere(playerController.weaponHoldingPoint.position, weapon.hitRange, enemyLayer);
            foreach (Collider enemy in potentialHitEnemies) {
                if (!allHitEnemies.Contains(enemy)) {
                    // Apply damage to the enemy
                    if (enemy.GetComponent<EnemyNpc>() != null) {
                        // Damage dealt is randomly set on each hit, defined between weapon's damage stats
                        int damage = UnityEngine.Random.Range(weapon.minDamage, weapon.maxDamage + 1);
                        Debug.Log("Applied " + damage + " to " + enemy);
                        enemy.GetComponent<EnemyNpc>().TakeDamage(damage);
                        allHitEnemies.Add(enemy);
                    }
                }
            }
            attackDuration -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Stop detection");
        playerController.playerAnimations.playerAnimator.speed = 1f;
    }

    void OnDrawGizmosSelected() {
        if (weapon != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerController.weaponHoldingPoint.position, weapon.hitRange);
        }
    }

    public void TakeDamage( float damage ) {
        if (playerController.isAlive) {
            //Check if cooldown for taking damage has passed

            //If player is blocking change the damage value based on player weapon blocking power
            if (playerController.isBlocking) {
                damage /= weapon.blockingPower;
                Debug.Log("Blocked");
            }
            //Substract armor value from damage
            damage = damage - playerController.stats.armor;
            Debug.Log("Player took " + damage + " dmg");
//
            //Perform death when health is depleted
            if (Time.time - lastDamageTime >= damageCooldown) {
                playerController.currentHealth -= damage;
                Debug.Log(playerController.currentHealth);
            } else {
                damage *= 0.2f;
                playerController.currentHealth -= damage;
            }
            if (playerController.currentHealth <= 0) {
                HandleDeath(playerController.playerAnimations);
            } else {
                playerController.playerAnimations.PlayHitAnimation();
                //PlayerData.Instance.UpdateHealth(playerController.currentHealth);
            }
            //Update last damage time
            lastDamageTime = Time.time;
        }
    }
    public void HandleDeath( PlayerAnimations playerAnimations ) {
        playerAnimations.PlayDeathAnimation();
        OnPlayerDeath?.Invoke();
        playerController.isAlive = false;
    }
}
