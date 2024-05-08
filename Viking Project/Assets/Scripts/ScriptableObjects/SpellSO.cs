using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]
public class SpellSO : ScriptableObject {
    public string spellName;
    public GameObject spellPrefab;
    public GameObject spellEffectPrefab;
    public float spellCooldown;
    public float spellLife;
    public float spellSpeed;

    private float lastActivationTime;

    private void OnEnable() {
        lastActivationTime = Time.time;
    }

    private void OnDisable() {
        lastActivationTime = 0;
    }

    public void ActivateAbility( Transform origin ) {
        if (Time.time - lastActivationTime >= spellCooldown) {
            // Execute the ability
            ExecuteAbility(origin);
            // Update the last activation time
            lastActivationTime = Time.time;
        } else {
            Debug.Log("Ability is on cooldown.");
        }
    }

    protected virtual void ExecuteAbility( Transform origin ) {
        // Implement the specific functionality of the spell in the subclasses
        Debug.Log("Spell functionality executed in spellSO.");
    }
}