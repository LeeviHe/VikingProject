using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ice Spell", menuName = "Abilities/Ice Spell")]
public class IceSpikeSO : SpellSO {
    public int damage;
    public float slowModifier;
    public int slowDuration;
    public LayerMask enemyLayer;

    public void IceSpike(Transform origin) {
        // Instantiate ability prefab at the specified position
        GameObject spell = Instantiate(spellPrefab, origin.position, origin.rotation);
        spell.GetComponent<Rigidbody>().velocity = origin.forward * spellSpeed;
        Destroy(spell, spellLife);
    }

    public void ApplySlowEffect( Collider enemy ) {
        EnemyNpc enemyNPC = enemy.GetComponent<EnemyNpc>();
        enemyNPC.activeSpell = this;
    }

    public IEnumerator Slow( EnemyNpc target ) {
        int timeSlowed = slowDuration;
        while (timeSlowed > 0) {
            target.navAgent.speed *= slowModifier;
            yield return new WaitForSeconds(1);
            timeSlowed--;
        }
        target.activeSpell = null;
    }

    protected override void ExecuteAbility( Transform origin ) {
        // Execute the functionality of the Fireball spell
        IceSpike(origin);
    }

}