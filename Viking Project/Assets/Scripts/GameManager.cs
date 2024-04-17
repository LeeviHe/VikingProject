using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject deathScreenUI;
    [SerializeField] private GameObject winScreenUI;
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private Toggle toggleUI;

    public float screenDelay = 2f;

    private void OnEnable() {
        Player.OnPlayerDeath += OnPlayerDeath;
        Player.OnPlayerWin += OnPlayerWin;
        Player.OnQuestActivated += OnQuestActivated;
    }

    private void OnDestroy() {
        Player.OnPlayerDeath -= OnPlayerDeath;
        Player.OnPlayerWin -= OnPlayerWin;
        Player.OnQuestActivated -= OnQuestActivated;
    }
    void OnPlayerDeath() {
        Invoke("ShowDeathScreen", screenDelay);
    }
    void OnQuestActivated() { 
        toggleUI.gameObject.SetActive(true);
    }
    void OnPlayerWin() {
        screenDelay = 0;
        Invoke("ShowWinScreen", screenDelay);
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
        toggleUI.isOn = true;
        /*Time.timeScale = 0f;
        winScreenUI.SetActive(true);
        playerHUD.SetActive(false);

        // You can add additional logic here, such as pausing the game or showing relevant information
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;*/
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