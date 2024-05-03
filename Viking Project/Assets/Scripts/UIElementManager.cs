using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementManager : MonoBehaviour {
    public GameObject blessingSelectorUI;
    public GameObject weaponSelectorUI;
    public GameObject inGameHud;
    public GameObject activeUI;

    public void ToggleScreen(GameObject selectorUI) {
        //When the selectorUI is active, toggle it as inactive, toggle player HUD off, continue game with timescale and clear activeUI since there isn't an active UI
        //When the selectorUI is not active, toggle it as active, toggle player HUD on, pause game with timescale and set the selectorUI as the activeUI object
        bool isActive = selectorUI.activeSelf;
        selectorUI.SetActive(!isActive);
        TogglePlayerHUD();
        if (isActive) {
            
            Time.timeScale = 1f;
            activeUI = null;
        } else { 
            Time.timeScale = 0f;
            activeUI = selectorUI.gameObject;
        }
    }

    public void SelectorBackButton() {
        ToggleScreen(activeUI);
        Time.timeScale = 1f;
    }

    public void TogglePlayerHUD() {
        bool isActive = inGameHud.activeSelf;
        inGameHud.SetActive(!isActive);
    }
}
