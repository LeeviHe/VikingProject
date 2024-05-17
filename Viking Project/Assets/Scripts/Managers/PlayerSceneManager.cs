using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneManager : MonoBehaviour {
    [SerializeField] private GameObject playerPrefab; // Reference to the player data
    public Transform playerSpawnPoint;
    private GameObject playerInstance; // Reference to the instantiated player object

    // Made for spawning player to level on scene change
    private void Start() {
        if (playerInstance == null && playerPrefab != null) {
            // Instantiate the player prefab
            playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
            // Camera follow the instantiated object
            Camera.main.GetComponent<CinemachineVirtualCamera>().Follow = playerInstance.transform;
        }
    }
}