using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementManager : MonoBehaviour {
    public GameObject blessingSelectorUI;
    public GameObject inGameHud;

    public void ToggleBlessingScreen() {
        bool isActive = blessingSelectorUI.activeSelf;

        blessingSelectorUI.SetActive(!isActive);
        TogglePlayerHUD();
    }

    public void TogglePlayerHUD() {
        bool isActive = inGameHud.activeSelf;
        inGameHud.SetActive(!isActive);
    }
}
