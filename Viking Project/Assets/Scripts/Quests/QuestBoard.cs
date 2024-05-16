using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBoard : MonoBehaviour, IInteractable {
    [SerializeField] private List<QuestSO> questList = new List<QuestSO>();
    public SelectorUIManager elementManager;
    string nextButtonName = "NextButton";
    string backButtonName = "PreviousButton";
    public int pageTotal;
    public int currentPage = 1;
    int elementsPerPage = 2;
    int questIteration = 0;
    int inspectedQuestIteration = 0;

    public void Awake() {
        CountPages();
        SetQuestsToPage(questIteration);
        elementManager.questPageText.text = currentPage + "/" + pageTotal;
    }

    public void InteractWithQuestBoard() {
        elementManager.ToggleScreen(elementManager.questBoardUI);
    }

    private void CountPages() {
        if (questList.Count > 0) {
            pageTotal = (int)Math.Ceiling((double)questList.Count / elementsPerPage);
        }
    }

    public void Navigation( Button buttonObject ) {
        if (buttonObject == elementManager.quest1) {
            inspectedQuestIteration = questIteration;
            SetQuestInfo(inspectedQuestIteration);
        } else if (buttonObject == elementManager.quest2) {
            inspectedQuestIteration = questIteration + 1;
            SetQuestInfo(inspectedQuestIteration);
        } else {
            if (buttonObject.name == nextButtonName && currentPage < pageTotal) {
                currentPage += 1;
                questIteration += 2;
                SetQuestsToPage(questIteration);
            } else if (buttonObject.name == backButtonName && currentPage > 1) {
                currentPage -= 1;
                questIteration -= 2;
                SetQuestsToPage(questIteration);
            }
            elementManager.questPageText.text = currentPage + "/" + pageTotal;
        }
        
    }

    private void SetQuestsToPage( int i ) {
        int first = i;
        int second = i + 1;
        elementManager.quest1Text.text = questList[first].name;
        if (questList.Count > second) {
            elementManager.quest2.gameObject.SetActive(true);
            elementManager.quest2Text.text = questList[second].name;
        } else {
            elementManager.quest2.gameObject.SetActive(false);
        }
    }

    private void SetQuestInfo(int i) {
        elementManager.questHeader.text = questList[i].name;
        elementManager.questDescription.text = questList[i].questDescription;
    }

    // Give the quest to the player and initialize quest items
    public void GiveQuestToPlayer( PlayerController player) {
        Debug.Log(player + " given quest " + questList[inspectedQuestIteration]);
        // Pass the quest to the player to start tracking
        player.ReceiveNewQuest(questList[inspectedQuestIteration]);
    }

    public void Interact( PlayerController player ) {
        InteractWithQuestBoard();
    }    
    public string GetInteractText() {
        return "Inspect Quest Board";
    }

    public Transform GetTransform() {
        return transform;
    }

}