using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkController : MonoBehaviour
{ 
    [Header("Others")]
    [SerializeField] private GridController grid;
    [SerializeField] private ScoreHandler scoreHandler;
    private int tilesRequiredForLink = 3; //This variable is fixed in this iteration, more than 3 would require recursive neighbour searching to find possible link paths within the grid.

    private Vector2[] directionsToCheck;

    public int TilesRequiredForLink { get => tilesRequiredForLink; }

    
    void Awake()
    {
        directionsToCheck = new Vector2[8];
        directionsToCheck[0] = Vector2.up;          //down for this grid
        directionsToCheck[1] = Vector2.down;        //up for this grid
        directionsToCheck[2] = Vector2.left;
        directionsToCheck[3] = Vector2.right;
        directionsToCheck[4] = Vector2.one;         //bottom right
        directionsToCheck[5] = new Vector2(1, -1);  //top right
        directionsToCheck[6] = new Vector2(-1, 1);  //bottom left
        directionsToCheck[7] = new Vector2(-1, -1); //top left
    }
    

    public bool HasGridValidLink()
    {
        for (int i = 0; i < grid.Cols; i++)
        {
            for (int j = 0; j < grid.Rows; j++)
            {
                if(CheckTileNeighboursForLink(i, j, tilesRequiredForLink))
                {
                    print("Possible link found at " + i + "," + j + "! Current grid is valid.");
                    return true;
                }
            }
        }

        print("Current grid is not valid, no possible links available");
        return false;
    }

    
    private bool CheckTileNeighboursForLink(int x, int y, int tilesRequiredForLink)
    {
        int amountOfNeighbourMatches = 0;

        if (!grid.GridTiles[x, y].IsChecked)
        {
            //Check neighbours in all 8 directions
            for (int i = 0; i < directionsToCheck.Length; i++)
            {
                amountOfNeighbourMatches += CheckNeighbourTile(x, y, directionsToCheck[i], grid.GridTiles[x, y].TileID);
            }

            //All neighbours checked
            grid.GridTiles[x, y].IsChecked = true;
            
            //Make up the score on current tile. In this case we have a minimum of 3 for a link so 2 neighbours means a link of 3 is always possible.
            if(amountOfNeighbourMatches < tilesRequiredForLink - 1)
                return false;
            else
                return true;
        }

        else
        {
            return false;
        }
    }

    /// <summary>
    /// Returns the amount of neighbouring tiles with the same ID (shape).
    /// </summary>
    private int CheckNeighbourTile(int tileX, int tileY, Vector2 directionToCheck, int currentTileID)
    {
        int amountOfNeighbourMatches = 0;

        if (CheckTileValidity(tileX + (int)directionToCheck.x, tileY + (int)directionToCheck.y))
        {
            if (grid.GridTiles[tileX + (int)directionToCheck.x, tileY + (int)directionToCheck.y].TileID == currentTileID)
            {
                //print("Matching neighbour found at: " + directionToCheck.ToString());
                amountOfNeighbourMatches++;
            }
        }

        return amountOfNeighbourMatches;
    }

    /// <summary>
    /// Used to check if the given tile coordinates fall within the grid.
    /// </summary>
    private bool CheckTileValidity(int x, int y)
    {
        if (x < 0 || y < 0 || x > grid.Cols - 1 || y > grid.Rows - 1)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Used during player input to determine if made move is valid.
    /// </summary>
    public bool IsTileMatchingNeighbour(Tile baseTile, Tile tileToCheck)
    {
        if (!baseTile.TileID.Equals(tileToCheck.TileID))
        {
            return false;
        }
        else //Same type, proceed to check if neighbour.
        {
            if(baseTile.TileCoordinates.x - tileToCheck.TileCoordinates.x > 1 || baseTile.TileCoordinates.x - tileToCheck.TileCoordinates.x < -1)
            {
                return false;
            }
            else if(baseTile.TileCoordinates.y - tileToCheck.TileCoordinates.y > 1 || baseTile.TileCoordinates.y - tileToCheck.TileCoordinates.y < -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
