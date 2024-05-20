using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   

public class MainMenu : MonoBehaviour {

    [SerializeField] private string hubSceneName;
    [SerializeField] private GameObject loadingScreen;

    public void StartGame() {
        StartCoroutine(LoadHubScene(hubSceneName));
    }

    private IEnumerator LoadHubScene( string levelName ) {
        // Show loading screen
        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // 0.9f is the maximum progress value
            Debug.Log("Loading progress: " + (progress * 100) + "%");
            yield return null;
        }
           // Hide loading screen
        loadingScreen.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
