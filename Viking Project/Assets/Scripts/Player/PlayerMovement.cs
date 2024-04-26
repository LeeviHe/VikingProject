using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private PlayerController playerController;
    private Vector3 moveDir;
    private Vector2 lookDir;
    public void UpdateMovementData( Vector3 newMovementDirection, Vector2 lookDirection ) {
        moveDir = newMovementDirection;
        lookDir = lookDirection;
        Debug.Log(lookDir);
    }
    private void Update() {
        MovePlayer();
        if (playerController.isFightMode && lookDir != Vector2.zero) {
            TurnPlayerFightMode();
        } else { 
            TurnPlayer();
        }
        
    }
    public void MovePlayer() {
        if (!playerController.isBlocking) {
            float moveDistance;
            moveDistance = playerController.currentMoveSpeed * Time.deltaTime;
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
    public void TurnPlayerFightMode() {
        float rotationSpeed = 5f;
        Vector3 lookRotation = new Vector3(lookDir.x, 0f, lookDir.y);
        if (lookDir.sqrMagnitude > 0.01f) {
            Quaternion targetRotation = Quaternion.LookRotation(-lookRotation, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
