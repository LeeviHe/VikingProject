using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {

    //This script is mostly obsolete, input is handled through editor and PlayerController

    private PlayerInputActions playerInputActions;
    //Enable new input system
    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    public void DisableInput() {
        playerInputActions.Player.Disable();
    }
    public void EnableInput() {
        playerInputActions.Player.Enable();
    }
    //Get movement values and normalize them
    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }
    //Set IsMoving as true while player is moving
    public bool IsMoving() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.magnitude > 0; // Check if magnitude of input vector is greater than 0
    }
    //Set IsAttacking as true if player attacks
    public bool AttackInput() {
        return playerInputActions.Player.Attack.triggered;
    }
    //Set IsBlocking while block input is being pressed
    public bool IsBlocking() {
        float blockInput = playerInputActions.Player.Block.ReadValue<float>();
        return blockInput > 0.5f; // Adjust the threshold as needed
    }
    public bool IsIdle() {
        return !IsMoving() && !AttackInput() && !IsBlocking();
    }
}
