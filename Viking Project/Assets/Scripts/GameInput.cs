using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {

    private PlayerInputActions playerInputActions;
    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }
    public bool IsAttacking() {
        return playerInputActions.Player.Attack.triggered;
    }
    public bool IsBlocking() {
        float blockInput = playerInputActions.Player.Block.ReadValue<float>();
        return blockInput > 0.5f; // Adjust the threshold as needed
    }
}
