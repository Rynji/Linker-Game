using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private List<Sprite> visualSprites;

    private int tileID;
    private bool isChecked;
    private Vector2 tileCoordinates;

    public int TileID { get => tileID; }
    public bool IsChecked { get => isChecked; set => isChecked = value; }
    public Vector2 TileCoordinates { get => tileCoordinates; set => tileCoordinates = value; }

    
    public void SetVisual()
    {
        tileID = Random.Range(0, visualSprites.Count);
        this.GetComponent<SpriteRenderer>().sprite = visualSprites[tileID];
    }

    public void ToggleLinkVisual(bool toggle)
    {
        this.transform.GetChild(0).gameObject.SetActive(toggle);
    }
}
