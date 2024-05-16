using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInitializer : MonoBehaviour {

    public PlayerData playerData;
    public List<Transform> spawnPoints; // List of available spawn points

    private List<Transform> availableSpawnPoints; // List of spawn points that are still available
    private void Awake() {
        playerData = PlayerData.Instance;
    }
    void Start() {
        availableSpawnPoints = new List<Transform>(spawnPoints);
        InitializeObjectives();
    }

    private void InitializeObjectives() {
        QuestSO activeQuest = playerData.currentQuest;

        if (activeQuest != null) {
            foreach (ObjectiveSO objective in activeQuest.objectives) {
                SpawnObjective(objective, objective.objectiveItemPrefab, activeQuest);
            }
        }
    }

    void SpawnObjective( ObjectiveSO objective, GameObject objectiveObject, QuestSO quest) {
        // Determine the position to spawn the objective
        Transform spawnPosition = GetRandomSpawnPosition();

        // Instantiate the objective prefab at the spawn position
        GameObject spawnedObjective = Instantiate(objectiveObject, spawnPosition.position, Quaternion.identity);

        // Optionally: Link the spawned objective to the quest objective
        spawnedObjective.GetComponent<QuestItem>().SetObjective(objective, quest);
    }

    private Transform GetRandomSpawnPosition() {
        int randomIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
        Transform spawnPoint = availableSpawnPoints[randomIndex];
        availableSpawnPoints.RemoveAt(randomIndex);
        return spawnPoint;
    }
}