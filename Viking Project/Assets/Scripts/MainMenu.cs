using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] private string hubSceneName;

    public void StartGame() {
        StartCoroutine(LoadHubScene(hubSceneName));
    }

    private IEnumerator LoadHubScene( string levelName ) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // 0.9f is the maximum progress value
            Debug.Log("Loading progress: " + (progress * 100) + "%");
            yield return null;
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
