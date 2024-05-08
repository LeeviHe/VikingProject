using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIElementManager : MonoBehaviour {
    public GameObject blessingSelectorUI;
    public GameObject weaponSelectorUI;
    public Image axeImage;
    public GameObject axeButton;
    public TextMeshProUGUI axeButtonText;
    public GameObject questBoardUI;
    public GameObject questInfoUI;
    public TextMeshProUGUI questPageText;
    public Button quest1;
    public TextMeshProUGUI quest1Text;
    public Button quest2;
    public TextMeshProUGUI quest2Text;
    public Button navButtonNext;
    public Button navButtonBack;
    public TextMeshProUGUI questHeader;
    public TextMeshProUGUI questDescription;
    public GameObject inGameHud;
    public GameObject activeUI;

    public void ToggleScreen( GameObject selectorUI ) {
        //When the selectorUI is active, toggle it as inactive, toggle player HUD off, continue game with timescale and clear activeUI since there isn't an active UI
        //When the selectorUI is not active, toggle it as active, toggle player HUD on, pause game with timescale and set the selectorUI as the activeUI object
        bool isActive = selectorUI.activeSelf;
        selectorUI.SetActive(!isActive);
        activeUI = selectorUI.gameObject;
        TogglePlayerHUD();

        if (activeUI) {
            Time.timeScale = 0.0f;
        } else {
            Time.timeScale = 1.0f;
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
