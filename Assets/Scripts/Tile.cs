using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private List<Sprite> visualSprites;

    private int randomID;

    public int RandomID { get => randomID; }

    
    public void SetVisual()
    {
        randomID = Random.Range(0, visualSprites.Count);
        this.GetComponent<SpriteRenderer>().sprite = visualSprites[randomID];
    }
}
