/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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