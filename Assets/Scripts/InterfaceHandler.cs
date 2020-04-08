using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceHandler : MonoBehaviour
{
    [Header("Externals")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text gameplayBottomText;
    [Header("Interals")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject levelSelectUI;
    [SerializeField] private GameObject gameUI;


    public void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        levelSelectUI.SetActive(false);
        gameUI.SetActive(false);
    }

    public void ShowLevelSelectUI()
    {
        mainMenuUI.SetActive(false);
        levelSelectUI.SetActive(true);
        gameUI.SetActive(false);
    }

    public void ShowGameUI()
    {
        mainMenuUI.SetActive(false);
        levelSelectUI.SetActive(false);
        gameUI.SetActive(true);
    }

    public void SetScoreDisplay(int scoreToDisplay)
    {
        scoreText.text = "Score: " + scoreToDisplay;
    }

    public void SetGameplayBottomText(string text)
    {
        gameplayBottomText.text = text;
    }
}
