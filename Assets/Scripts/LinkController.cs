using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkController : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private GridController grid;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            FindLinks();
        }
    }

    private void FindLinks()
    {
        // for (int i = 0; i < grid.Cols; i++)
        // {
        //     for (int j = 0; j < grid.Rows; j++)
        //     {
        //         CheckNeighbourTiles(i, j);
        //         //print("Tile: " + grid.GridTiles[i, j].name + " i,j: " + i + "," + j);
        //     }
        // }

        CheckNeighbourTiles(0, 0);
    }

    //TODO: Group into methods/
    private void CheckNeighbourTiles(int x, int y)
    {
        if (!grid.GridTiles[x, y].IsChecked)
        {
            int currentID = grid.GridTiles[x, y].TileID;

            //Check Top = 0,-1
            if (CheckTileValidity(x, y - 1))
            {
                if (grid.GridTiles[x, y - 1].TileID == currentID)
                {
                    print("Matching neighbour found: TOP");
                    //Check neighbours of matched neighbour //Need some way to store current link when going recursive.
                    CheckNeighbourTiles(x, y - 1);
                }
            }
            else
                print("Top Tile not valid");

            //Check Top Right = 1,-1
            if (CheckTileValidity(x + 1, y - 1))
            {
                if (grid.GridTiles[x + 1, y - 1].TileID == currentID)
                {
                    print("Matching neighbour found: TOP RIGHT");
                }
            }
            else
                print("Top Right not valid");

            //Check Right = 1,0
            if (CheckTileValidity(x + 1, y))
            {
                if (grid.GridTiles[x + 1, y].TileID == currentID)
                {
                    print("Matching neighbour found: RIGHT");
                }
            }
            else
                print("Right not valid");

            //Check Bottom Right = 1,1
            if (CheckTileValidity(x + 1, y + 1))
            {
                if (grid.GridTiles[x + 1, y + 1].TileID == currentID)
                {
                    print("Matching neighbour found: BOTTOM RIGHT");
                }
            }
            else
                print("Bottom Right not valid");

            //Check Bottom = 0,1
            if (CheckTileValidity(x, y + 1))
            {
                if (grid.GridTiles[x, y + 1].TileID == currentID)
                {
                    print("Matching neighbour found: BOTTOM");
                }
            }
            else
                print("Bottom not valid");

            //Check Bottom Left = -1,1
            if (CheckTileValidity(x - 1, y + 1))
            {
                if (grid.GridTiles[x - 1, y + 1].TileID == currentID)
                {
                    print("Matching neighbour found: BOTTOM LEFT");
                }
            }
            else
                print("Bottom Left not valid");

            //Check Left = -1,0
            if (CheckTileValidity(x - 1, y))
            {
                if (grid.GridTiles[x - 1, y].TileID == currentID)
                {
                    print("Matching neighbour found: LEFT");
                }
            }
            else
                print("Left not valid");

            //Check Top Left = -1,-1
            if (CheckTileValidity(x - 1, y - 1))
            {
                if (grid.GridTiles[x - 1, y - 1].TileID == currentID)
                {
                    print("Matching neighbour found: TOP LEFT");
                }
            }
            else
                print("Top Left not valid");

            //All neighbours checked
            grid.GridTiles[x, y].IsChecked = true;
            //Make up score on current link
            
        }

        else
        {
            return;
        }
    }

    private bool CheckTileValidity(int x, int y)
    {
        if (x < 0 || y < 0 || x > grid.Cols - 1 || y > grid.Rows - 1)
            return false;
        else
            return true;
    }
}
