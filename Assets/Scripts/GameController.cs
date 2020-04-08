using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private InterfaceHandler interfaceHandler;
    [SerializeField] private ScoreHandler scoreHandler;
    [SerializeField] private GridController gridController;


    void Start()
    {
        //Display Main Menu
        interfaceHandler.ShowMainMenu();
    }

    public void SetLevelActive(/*TODO: Level Properties like background, grid size etc.*/)
    {
        interfaceHandler.ShowGameUI();
        gridController.FillGrid();
    }
}
