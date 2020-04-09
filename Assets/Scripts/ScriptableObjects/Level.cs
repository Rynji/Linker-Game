using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MAG/Match 3 Level")]
public class Level : ScriptableObject
{
    public Sprite backgroundImage;
    public GameObject tilePrefab;
    public int scoreRequired;
}
