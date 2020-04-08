using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private List<Sprite> visualSprites;

    private int tileID;
    private bool isChecked; //Unused for now, can be used when checking tiles recursively for links paths if we ever need more than 3 minimum.
    private bool isCompleted;
    private Vector2 tileCoordinates;

    public int TileID { get => tileID; set => tileID = value; }
    public bool IsChecked { get => isChecked; set => isChecked = value; }
    public Vector2 TileCoordinates { get => tileCoordinates; set => tileCoordinates = value; }
    public bool IsCompleted { get => isCompleted; set => isCompleted = value; }

    
    public void SetVisual()
    {
        this.tileID = Random.Range(0, visualSprites.Count);
        this.GetComponent<SpriteRenderer>().sprite = visualSprites[tileID];
    }

    public void SetVisualForced(int visualID)
    {
        this.tileID = visualID;
        this.GetComponent<SpriteRenderer>().sprite = visualSprites[tileID];
    }

    public void ToggleLinkVisual(bool toggle)
    {
        this.transform.GetChild(0).gameObject.SetActive(toggle);
    }
}
