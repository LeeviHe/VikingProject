using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private PlayerController playerController;
    private Vector3 moveDir;
    public void UpdateMovementData( Vector3 newMovementDirection ) {
        moveDir = newMovementDirection;
    }
    private void Update() {
        MovePlayer();
        TurnPlayer();
    }
    public void MovePlayer() {
        if (!playerController.isBlocking) {
            float moveDistance = playerController.currentMoveSpeed * Time.deltaTime;
            transform.Translate(moveDir * moveDistance, Space.World);
        }
    }

    public void TurnPlayer() {
        float rotationSpeed = 10f;
        if (moveDir.sqrMagnitude > 0.01f) {
            Quaternion targetRotation = Quaternion.LookRotation(-moveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
