using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    private enum GameState { 
        Default,
        QuestActive,
        ReadyToLeave,
        GameOver
    }

    [SerializeField] private GameObject playerHUD;
    [SerializeField] private Toggle toggleUI;
    [SerializeField] private GameObject portal;
    GameState gameState;
    public float screenDelay;
    public bool isPaused;
    

    private void Start() {
        gameState = GameState.Default;
        isPaused = false;
    }
    private void Update() {
        HandleGameState();
    }


    // Remnant of old demo, little functionality
    private void HandleGameState() {
        // Handle logic based on current state
        switch (gameState) {
            case GameState.Default:
                break;
            case GameState.QuestActive:
                break;
            case GameState.ReadyToLeave:
                portal.gameObject.SetActive(true);
                break;
            case GameState.GameOver:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
        }
    }
    private void OnEnable() {
        PlayerCombat.OnPlayerDeath += OnPlayerDeath;
        PlayerController.OnQuestActivated += OnQuestActivated;
        PlayerController.OnReadyToLeave += OnReadyToLeave;
    }

    private void OnDestroy() {
        PlayerCombat.OnPlayerDeath -= OnPlayerDeath;
        PlayerController.OnQuestActivated -= OnQuestActivated;
        PlayerController.OnReadyToLeave -= OnReadyToLeave;
    }
    void OnPlayerDeath() {
        gameState = GameState.GameOver;
    }
    void OnQuestActivated() {
        gameState = GameState.QuestActive;
    }
    void OnReadyToLeave() {
        gameState = GameState.ReadyToLeave;
    }


    // To be used for pausing the game
    public void TogglePauseState() {
        isPaused = !isPaused;

        ToggleTimeScale();
    }

    void ToggleTimeScale() {
        float newTimeScale = 0f;

        switch (isPaused) {
            case true:
                newTimeScale = 0f;
                break;

            case false:
                newTimeScale = 1f;
                break;
        }

        Time.timeScale = newTimeScale;
    }
}
