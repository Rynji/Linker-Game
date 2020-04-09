using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceHandler : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [Header("Text")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text gametypeText;
    [SerializeField] private Text gameplayBottomText, popupText;
    [Header("UI Elements")]
    [SerializeField] private GameObject mainMenuUI; 
    [SerializeField] private GameObject levelSelectUI, gameUI, popupUI;
    [SerializeField] private Transform levelButtonGroupParent;
    [Header("UI Visuals")]
    [SerializeField] private SpriteRenderer gameplayBackground; 
    [Header("Buttons")]
    [SerializeField] private GameObject retryButton; 
    [SerializeField] private GameObject levelSelectButton;
    [Header("Prefabs")]
    [SerializeField] private GameObject levelButtonPrefab;


    //UI Display Methods

    public void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        levelSelectUI.SetActive(false);
        gameUI.SetActive(false);
        gameplayBackground.sprite = null;
    }

    public void ShowLevelSelectUI()
    {
        mainMenuUI.SetActive(false);
        levelSelectUI.SetActive(true);
        gameUI.SetActive(false);
        gameplayBackground.sprite = null;
    }

    public void ShowGameUI()
    {
        mainMenuUI.SetActive(false);
        levelSelectUI.SetActive(false);
        gameUI.SetActive(true);
    }

    public void ShowPopup(string textToDisplay, bool showRetryButton)
    {
        popupUI.SetActive(true);
        popupText.text = textToDisplay;

        if(showRetryButton)
            retryButton.SetActive(true);
        else
            levelSelectButton.SetActive(true);
    }

    //Text Methods

    public void SetScoreDisplay(int oldScore, int scoreToDisplay)
    {
        //oldScore can be used to nicely increment to score displayed.
        scoreText.text = "Score: " + scoreToDisplay;
    }

    public void SetGametypeText(string gametypeTextToDisplay)
    {
        gametypeText.text = gametypeTextToDisplay;
    }

    public void SetMovesLeftText(int maxMovesAllowed, int movesTaken)
    {
        gameplayBottomText.text = "Moves Left: " + (maxMovesAllowed - movesTaken);
        
        if(maxMovesAllowed == -1)
            gameplayBottomText.text = "Get the required score to win!";
        else if(maxMovesAllowed - movesTaken <= 0)
            gameplayBottomText.text = "No more moves left!";
    }

    //Buttons/actions

    public void OnRetryButtonClicked()
    {
        gameController.SetLevelActive(gameController.CurrentLevel);
        ClosePopup();
    }

    public void OnLevelSelectButtonClicked()
    {
        gameController.CloseCurrentLevel();
        ClosePopup();
        ShowLevelSelectUI();
    }

    public void SetGameBackground(Sprite backgroundSprite)
    {
        gameplayBackground.sprite = backgroundSprite;
    }

    public void InstantiateLevelButtons(Level level)
    {
        GameObject levelButton = Instantiate(levelButtonPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, levelButtonGroupParent);
        levelButton.transform.GetChild(0).GetComponent<Text>().text = level.levelName;
        levelButton.transform.GetChild(1).GetComponent<Text>().text = "Target: " + String.Format("{0:n0}", level.scoreRequired);
        
        if(level.maxMovesAllowed > 0)
            levelButton.transform.GetChild(2).GetComponent<Text>().text = "Max Moves: " + level.maxMovesAllowed;
        else
            levelButton.transform.GetChild(2).GetComponent<Text>().text = "Unlimited Moves";
        
        levelButton.GetComponent<Button>().onClick.AddListener(delegate { gameController.SetLevelActive(level); });
        levelButton.GetComponent<Image>().color = level.levelButtonColour;
    }

    public void ClosePopup()
    {
        popupUI.SetActive(false);
        retryButton.SetActive(false);
        levelSelectButton.SetActive(false);
    }
}
