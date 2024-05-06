using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private enum GameState { 
        Default,
        QuestActive,
        ReadyToLeave,
        GameOver,
        GameWon
    }
    [SerializeField] private GameObject deathScreenUI;
    [SerializeField] private GameObject winScreenUI;
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private Toggle toggleUI;
    [SerializeField] private GameObject portal;
    GameState gameState;
    public float screenDelay;
    

    private void Start() {
        gameState = GameState.Default;
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
            case GameState.GameWon:
                screenDelay = 0.5f;
                //Invoke("ShowWinScreen", screenDelay);
                break;
        }
    }
    private void OnEnable() {
        PlayerCombat.OnPlayerDeath += OnPlayerDeath;
        //PlayerController.OnPlayerWin += OnPlayerWin;
        PlayerController.OnQuestActivated += OnQuestActivated;
        PlayerController.OnReadyToLeave += OnReadyToLeave;
    }

    private void OnDestroy() {
        PlayerCombat.OnPlayerDeath -= OnPlayerDeath;
        //PlayerController.OnPlayerWin -= OnPlayerWin;
        PlayerController.OnQuestActivated -= OnQuestActivated;
        PlayerController.OnReadyToLeave -= OnReadyToLeave;
    }
    void OnPlayerDeath() {
        gameState = GameState.GameOver;
    }
    void OnQuestActivated() {
        gameState = GameState.QuestActive;
    }
    void OnPlayerWin() {
        gameState = GameState.GameWon;
    }
    void OnReadyToLeave() {
        gameState = GameState.ReadyToLeave;
    }
    private void ShowDeathScreen() {
        Time.timeScale = 0f;
        deathScreenUI.SetActive(true);
        playerHUD.SetActive(false);

        // You can add additional logic here, such as pausing the game or showing relevant information
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void ShowWinScreen() {
        Time.timeScale = 0f;
        winScreenUI.SetActive(true);
        playerHUD.SetActive(false);

        // You can add additional logic here, such as pausing the game or showing relevant information
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
