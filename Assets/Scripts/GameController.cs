using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Class References")]
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private InterfaceHandler interfaceHandler;
    [SerializeField] private ScoreHandler scoreHandler;
    [SerializeField] private GridController gridController;
    [Header("Levels")]
    [SerializeField] private List<Level> levels;

    private Level currentLevel;

    public Level CurrentLevel { get => currentLevel; }

    
    void Start()
    {
        interfaceHandler.ShowMainMenu();

        for (int i = 0; i < levels.Count; i++)
        {
            interfaceHandler.InstantiateLevelButtons(levels[i]);
        }
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
        scoreHandler.MaxMovesAllowed = level.maxMovesAllowed;
        if(level.maxMovesAllowed > 0)
            scoreHandler.OnMovesIncremented += interfaceHandler.SetMovesLeftText;
        scoreHandler.OnScoreChanged += interfaceHandler.SetScoreDisplay;
        scoreHandler.OnEnoughScored += OnGameWin;
        scoreHandler.OnMaxMovesUsed += OnGameLoss;

        interfaceHandler.SetGametypeText("Goal: " + String.Format("{0:n0}", level.scoreRequired));
        interfaceHandler.SetMovesLeftText(level.maxMovesAllowed, 0);

        gridController.TilePrefab = level.tilePrefab;
        gridController.FillGrid();
        inputHandler.GameEnd = false;
        inputHandler.LockInput = false;
    }

    public void CloseCurrentLevel()
    {
        scoreHandler.ResetScore();
        scoreHandler.ResetMoves();
        scoreHandler.OnScoreChanged -= interfaceHandler.SetScoreDisplay;
        gridController.ResetGridController();
        interfaceHandler.ClosePopup();
        interfaceHandler.ShowLevelSelectUI();
    }

    private void OnGameWin()
    {
        scoreHandler.OnEnoughScored -= OnGameWin;
        scoreHandler.OnMaxMovesUsed -= OnGameLoss;

        interfaceHandler.ShowPopup("Level Completed!", false);
        inputHandler.GameEnd = true;
    }

    private void OnGameLoss()
    {
        scoreHandler.OnEnoughScored -= OnGameWin;
        scoreHandler.OnMaxMovesUsed -= OnGameLoss;

        interfaceHandler.ShowPopup("Level Failed!", true);
        inputHandler.GameEnd = true;
    }
}
