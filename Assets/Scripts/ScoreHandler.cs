using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    public event Action OnEnoughScored, OnMaxMovesUsed; 
    public event Action<int, int> OnScoreChanged, OnMovesIncremented;
    
    [Header("Others")]
    [SerializeField] private InputHandler inputHandler;
    [Header("Gameplay Variables")]
    [SerializeField] private int[] scoringTable = new int[10];
    [SerializeField] private int flatScoreOnMaxLink;

    private int currentScore, scoreRequired, movesTaken, maxMovesAllowed;

    public int ScoreRequired { get => scoreRequired; set => scoreRequired = value; }
    public int MaxMovesAllowed { get => maxMovesAllowed; set => maxMovesAllowed = value; }


    void Start()
    {
        inputHandler.OnLinkSuccesfull += ScoreLink; 
    }

    /// <summary>
    /// The list defines the progress of the score depending on list length
    /// After the pre-defined score progress has run out add a flat score for every extra length
    /// </summary>
    public void ScoreLink(int linkLength)
    {
        int previousScore = currentScore;

        if(linkLength - 3 < scoringTable.Length)
            currentScore += scoringTable[linkLength - 3];
        else
            currentScore += scoringTable[scoringTable.Length - 1] + flatScoreOnMaxLink * (linkLength - 3 - (scoringTable.Length - 1));

        if(OnScoreChanged != null)
            OnScoreChanged(previousScore, currentScore);

        CheckCurrentScore();
    }

    public void IncrementMovesTaken()
    {
        movesTaken++;
        if(maxMovesAllowed > 0 && movesTaken >= maxMovesAllowed)
        {
            if(OnMaxMovesUsed != null)
                OnMaxMovesUsed();
        }
        if(OnMovesIncremented != null)
            OnMovesIncremented(maxMovesAllowed, movesTaken);
    }

    public void ResetScore()
    {
        currentScore = 0;
        if(OnScoreChanged != null)
            OnScoreChanged(0, 0);
    }

    public void ResetMoves()
    {
        movesTaken = 0;
    }

    private void CheckCurrentScore()
    {
        if(currentScore >= scoreRequired)
        {
            if(OnEnoughScored != null)
                OnEnoughScored();
        }
    }
}
