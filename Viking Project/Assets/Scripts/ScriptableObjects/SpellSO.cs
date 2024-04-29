using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]
public class SpellSO : ScriptableObject {
    public string spellName;
    public GameObject spellPrefab;
    public float spellCooldown;

    private float lastActivationTime;

    public void ActivateAbility( Transform origin ) {
        if (Time.time - lastActivationTime >= spellCooldown) {
            // Instantiate ability prefab at the specified position
            GameObject effect = Instantiate(spellPrefab, origin.position, origin.rotation);
            Destroy(effect, 0.5f);

            // Update the last activation time
            lastActivationTime = Time.time;

            // Execute the ability
            ExecuteAbility(origin);
        } else {
            Debug.Log("Ability is on cooldown.");
        }
    }

    protected virtual void ExecuteAbility( Transform origin ) {
        // Implement the specific functionality of the spell in the subclasses
        Debug.Log("Spell functionality executed in spellSO.");
    }
}