using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private List<Sprite> visualSprites;

    private int tileID;
    private bool isChecked;

    public int TileID { get => tileID; }
    public bool IsChecked { get => isChecked; set => isChecked = value; }

    
    public void SetVisual()
    {
        tileID = Random.Range(0, visualSprites.Count);
        this.GetComponent<SpriteRenderer>().sprite = visualSprites[tileID];
    }
}
