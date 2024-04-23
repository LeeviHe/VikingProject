using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour, IInteractable {
    [Header("Strings for Scene and UI")]
    [SerializeField] private string sceneName;
    [SerializeField] private string hubName;

    public void SwitchToHubScene() {
        StartCoroutine(LoadLevelAsync(sceneName));
    }

    private IEnumerator LoadLevelAsync( string levelName ) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // 0.9f is the maximum progress value
            Debug.Log("Loading progress: " + (progress * 100) + "%");
            yield return null;
        }
    }
    public string GetInteractText() {
        return "Exit to " + hubName;
    }

    public Transform GetTransform() {
        return transform;
    }

    public void Interact( PlayerController player ) {
        SwitchToHubScene();
    }
}