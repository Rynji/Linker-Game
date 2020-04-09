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
    [Header("UI Visuals")]
    [SerializeField] private SpriteRenderer gameplayBackground; 
    [Header("Buttons")]
    [SerializeField] private GameObject retryButton; 
    [SerializeField] private GameObject levelSelectButton;


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

    public void SetGameplayBottomText(string text)
    {
        gameplayBottomText.text = text;
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

    private void ClosePopup()
    {
        popupUI.SetActive(false);
        retryButton.SetActive(false);
        levelSelectButton.SetActive(false);
    }
}
