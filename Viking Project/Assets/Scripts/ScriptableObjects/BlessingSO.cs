using UnityEngine;

[CreateAssetMenu(fileName = "New Blessing", menuName = "Blessings/Blessing")]
public class BlessingSO : ScriptableObject {
    public string blessingName;
    public float movementSpeedModifier;
    public int healthModifier;
    public bool canWieldTwoHanded;
    public SpellSO[] specialAbilities;
    // Add additional fields for special abilities if needed

    public void ApplyBlessing( PlayerController playerController ) {
        playerController.ModifyMovementSpeed(movementSpeedModifier);
        playerController.ModifyHealth(healthModifier);
        playerController.canWieldTwoHanded = canWieldTwoHanded;
        // Apply other blessing effects here
    }
}
