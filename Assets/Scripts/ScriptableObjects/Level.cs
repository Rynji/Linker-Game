using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MAG/Match 3 Level")]
public class Level : ScriptableObject
{
    [Header("Strings")]
    public string levelName;
    [Header("Visuals")]
    public Sprite backgroundImage;
    public Color levelButtonColour;
    [Header("Audio")]
    public AudioClip gridRefillClip;
    public AudioClip levelWonClip, levelLostClip;
    [Header("Tile used in this level")]
    public GameObject tilePrefab;
    [Header("Game score & Max moves")]
    public int scoreRequired;
    public int maxMovesAllowed = -1;
}
