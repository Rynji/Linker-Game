using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private InterfaceHandler interfaceHandler;
    [SerializeField] private ScoreHandler scoreHandler;
    [SerializeField] private GridController gridController;

    private Level currentLevel;

    public Level CurrentLevel { get => currentLevel; }

    
    void Start()
    {
        interfaceHandler.ShowMainMenu();
    }

    public void SetLevelActive(Level level)
    {
        gridController.ResetGridController();
        scoreHandler.ResetScore();
        interfaceHandler.ShowGameUI();

        //Level specifics
        this.currentLevel = level;

        interfaceHandler.SetGameBackground(level.backgroundImage);

        scoreHandler.ScoreRequired = level.scoreRequired;
        scoreHandler.OnScoreChanged += interfaceHandler.SetScoreDisplay;
        scoreHandler.OnEnoughScored += OnGameWin;

        interfaceHandler.SetGametypeText("Goal: " + String.Format("{0:n0}", level.scoreRequired));

        gridController.TilePrefab = level.tilePrefab;
        gridController.FillGrid();
    }

    public void CloseCurrentLevel()
    {
        scoreHandler.ResetScore();
        scoreHandler.OnScoreChanged -= interfaceHandler.SetScoreDisplay;
        gridController.ResetGridController();
        interfaceHandler.ShowLevelSelectUI();
    }

    private void OnGameWin()
    {
        scoreHandler.OnEnoughScored -= OnGameWin;
        interfaceHandler.ShowPopup("Level Completed!", false);
    }

    private void OnGameLoss()
    {
        //Event OnLoss()
        interfaceHandler.ShowPopup("Level Completed!", true);
    }
}
