using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectorUIManager : MonoBehaviour {
    [Header("Blessing selection UI")]
    public GameObject blessingSelectorUI;
    [Header("Weapon selection UI")]
    public GameObject weaponSelectorUI;
    public Image axeImage;
    public GameObject axeButton;
    public TextMeshProUGUI axeButtonText;
    [Header("Quest board UI")]
    public GameObject questBoardUI;
    public GameObject questInfoUI;
    public TextMeshProUGUI questPageText;
    public Button quest1;
    public TextMeshProUGUI quest1Text;
    public Button quest2;
    public TextMeshProUGUI quest2Text;
    public Button navButtonNext;
    public Button navButtonBack;
    [Header("Quest detail UI")]
    public TextMeshProUGUI questHeader;
    public TextMeshProUGUI questDescription;
    [Header("Shop UI")]
    public GameObject shopScreen;
    public TextMeshProUGUI playerMoneyScreenText;
    public TextMeshProUGUI potionCountText;
    [Header("Player HUD")]
    public GameObject inGameHud;
    [Header("Active UI element")]
    public GameObject activeUI;

    public void ToggleScreen( GameObject selectorUI ) {
        //Use for toggling UI screen such as weapon selection screen
        //Set wanted UI GameObject from above fields as selectorUI
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

    //Button for universal back button on UI screens
    public void SelectorBackButton() {
        ToggleScreen(activeUI);
        Time.timeScale = 1f;
    }

    public void TogglePlayerHUD() {
        bool isActive = inGameHud.activeSelf;
        inGameHud.SetActive(!isActive);
    }
}
