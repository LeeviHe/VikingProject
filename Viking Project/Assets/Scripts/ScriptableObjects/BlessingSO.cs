using UnityEngine;

[CreateAssetMenu(fileName = "New Blessing", menuName = "Blessings/Blessing")]
public class BlessingSO : ScriptableObject {
    public string blessingName;
    public float movementSpeedModifier;
    public int healthModifier;
    public bool hasSpecialAbility;
    // Add additional fields for special abilities if needed

    public void ApplyBlessing( PlayerController playerController ) {
        playerController.ModifyMovementSpeed(movementSpeedModifier);
        playerController.ModifyHealth(healthModifier);
        // Apply other blessing effects here
    }
}
