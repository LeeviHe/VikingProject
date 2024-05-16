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
    [SerializeField] private GameObject deathScreenUI;
    [SerializeField] private GameObject winScreenUI;
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


    private void HandleGameState() {
        // Handle logic based on current state
        switch (gameState) {
            case GameState.Default:
                //Cursor.visible = false;
                break;
            case GameState.QuestActive:
                //toggleUI.gameObject.SetActive(true);
                break;
            case GameState.ReadyToLeave:
                //toggleUI.isOn = true;
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

    public void RestartButtonClicked() {
        // Unpause the game
        Time.timeScale = 1f;

        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitButtonClicked() {
        // Implement quit button logic here, such as quitting the application
        // Add your quit button logic here
        Application.Quit();
    }
}
