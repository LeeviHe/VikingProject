using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAnimations : MonoBehaviour {
    public Animator playerAnimator;

    private int playerMovementAnimationID;
    private int playerAttackAnimationID;
    private int playerBlockAnimationID;
    private int playerHitAnimationID;
    private int playerDeathAnimationID;

    private void Awake() {
        SetupAnimationIDs();
    }
    void SetupAnimationIDs() {
        playerMovementAnimationID = Animator.StringToHash("Movement");
        playerAttackAnimationID = Animator.StringToHash("Attack");
        playerBlockAnimationID = Animator.StringToHash("Block");
        playerHitAnimationID = Animator.StringToHash("Damage");
        playerDeathAnimationID = Animator.StringToHash("Dying");
    }
    public void UpdateMovementAnimation( float movementBlendValue ) {
        playerAnimator.SetFloat(playerMovementAnimationID, movementBlendValue);
    }
    public void PlayAttackAnimation() {
        playerAnimator.SetTrigger(playerAttackAnimationID);
    }
    public void PlayBlockAnimation( bool value ) {
        playerAnimator.SetBool(playerBlockAnimationID, value);
    }
    public void PlayHitAnimation() {
        playerAnimator.SetTrigger(playerHitAnimationID);
    }
    public void PlayDeathAnimation() {
        playerAnimator.SetTrigger(playerDeathAnimationID);
    }
}