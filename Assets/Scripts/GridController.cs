using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public event Action OnFillCompleted, OnGridShuffle;

    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private int rows, cols = 0;
    [SerializeField]
    private float tileSize;
    [Header("Debug Tools")]
    [SerializeField]
    private bool setGridUpdate;

    private Tile[,] gridTiles;
    private List<Tile> completedLink;
    private List<Tile> columnFillTiles;
    private LinkController linkController;

    public Tile[,] GridTiles
    {
        get
        {
            return gridTiles;
        }

        set
        {
            gridTiles = value;
        }
    }
    public int Rows { get => rows; }
    public int Cols { get => cols; }
    public List<Tile> CompletedLink { get => completedLink; set => completedLink = value; }

    
    void Start()
    {
        linkController = this.GetComponent<LinkController>();
        columnFillTiles = new List<Tile>();
        completedLink = new List<Tile>();

        gridTiles = new Tile[cols, rows];

        FillGrid();
    }

    void Update()
    {
        //Used at the beginning of development to determine grid size/positioning.
        if (setGridUpdate)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    gridTiles[i, j].transform.position = GetGridPosition(i, j);
                }
            }
        }
    
        if(Input.GetKeyDown(KeyCode.R))
        {
            ClearGrid();
            FillGrid();
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            DebugGrid();
        }
    }

    private void DebugGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            print("Row " + i + " has ID: " + gridTiles[0, i].TileID);
        }
    }

    private void FillGrid()
    {
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject go = Instantiate(tilePrefab, GetGridPosition(i, j), Quaternion.identity, this.gameObject.transform);
                go.name = "Tile_" + i + "_" + j;
                gridTiles[i, j] = go.GetComponent<Tile>();
                gridTiles[i, j].SetVisual();
                gridTiles[i, j].TileCoordinates = new Vector2(i, j);
            }
        }

        if(!linkController.HasGridValidLink())
        {
            if(OnGridShuffle != null)
                OnGridShuffle();

            ClearGrid();
            FillGrid();
        }
    }

    private void ClearGrid()
    {
        //Since the grid can be changed during runtime just clear all the tiles manually instead of going by rows/cols.
        for (int i = transform.childCount; i --> 0;)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }

        Array.Clear(gridTiles, 0, gridTiles.Length);
    }

    private Vector3 GetGridPosition(int i, int j)
    {
        Vector3 gridPos = new Vector3();

        gridPos = this.gameObject.transform.localPosition + new Vector3((i * tileSize), (-j * tileSize), 0f);

        return gridPos;
    }

    public IEnumerator RefillGrid()
    {
        for (int colIndex = 0; colIndex < cols; colIndex++)
        {
            int emptySpotsInCol = 0;
            int emptySpotIndex = -1;
            bool lookingForBlock = false;

            for (int i = 0; i < rows; i++)
            {
                if (gridTiles[colIndex, i].IsCompleted)
                {
                    emptySpotsInCol++;
                }
            }
            
            //print(emptySpotsInCol + " empty spots found.");

            //Spawn new Blocks that are needed above the grid
            for (int i = 0; i < emptySpotsInCol; i++)
            {
                GameObject go = Instantiate(tilePrefab, GetGridPosition(colIndex, -i - 1), Quaternion.identity, this.gameObject.transform);
                columnFillTiles.Add(go.GetComponent<Tile>());
                columnFillTiles[i].SetVisual();
            }

            //Collapse down existing blocks on empty spots
            for (int j = rows; j-- > 0;) //reversed loop as I want to work from the bottom up.
            {
                //print("Now checking j: " + j);
                if (gridTiles[colIndex, j].IsCompleted && !lookingForBlock)
                {
                    emptySpotIndex = j;
                    //print("Empty spot found at: " + emptySpotIndex);

                    //First empty spot encountered, this empty spot needs to grab a block from above before moving on.
                    //So first thing: we find the nearest not empty block and place it at our empty position.
                    lookingForBlock = true;

                    //else -> loop up until non empty block found or < 0 reached
                    //non-empty block found: move it to empty position
                    //< 0 reached: Grab first block from columnFillTiles[] under here.

                }
                else if (gridTiles[colIndex, j].IsCompleted && lookingForBlock)
                {
                    //print("Looking for block...");
                }
                else if (!gridTiles[colIndex, j].IsCompleted && lookingForBlock) //Block not empty and looking for a block
                {
                    //Move first not-empty tile to registered empty index in Tile array
                    gridTiles[colIndex, emptySpotIndex].TileID = gridTiles[colIndex, j].TileID;
                    gridTiles[colIndex, emptySpotIndex].TileCoordinates = gridTiles[colIndex, j].TileCoordinates;

                    //Remove (TODO: move down animation) old Tile
                    Destroy(gridTiles[colIndex, j].gameObject);
                    gridTiles[colIndex, j].IsCompleted = true; //Set old tile as empty again so it can be used on the next step

                    //Instantiate the tile on the empty position
                    GameObject go = Instantiate(tilePrefab, GetGridPosition(colIndex, emptySpotIndex), Quaternion.identity, this.gameObject.transform);
                    go.name = "Tile_" + colIndex + "_" + emptySpotIndex;
                    //Assign variables over that are stored in the array (could make deep copy here? I just copy the values myself)
                    go.GetComponent<Tile>().TileID = gridTiles[colIndex, emptySpotIndex].TileID;
                    go.GetComponent<Tile>().TileCoordinates = new Vector2(colIndex, emptySpotIndex);
                    gridTiles[colIndex, emptySpotIndex] = go.GetComponent<Tile>();
                    gridTiles[colIndex, emptySpotIndex].SetVisualForced(gridTiles[colIndex, emptySpotIndex].TileID);

                    //Now reset back to above newest placed block.
                    lookingForBlock = false;
                    j = rows - 1;
                    emptySpotIndex = -1;
                }
            }

            yield return new WaitForSeconds(0.1f);

            //Collapse down new blocks on empty spots (if needed)
            if (columnFillTiles.Count > 0)
            {
                //Flip the list (this is needed to get the blocks in the right order top to bottom)
                columnFillTiles.Reverse();

                for (int i = 0; i < columnFillTiles.Count; i++)
                {
                    //instantiate
                    GameObject go = Instantiate(tilePrefab, GetGridPosition(colIndex, i), Quaternion.identity, this.gameObject.transform);
                    go.name = "Tile_" + colIndex + "_" + i;
                    go.GetComponent<Tile>().TileID = columnFillTiles[i].TileID;
                    go.GetComponent<Tile>().TileCoordinates = new Vector2(colIndex, i);
                    //assign to grid array
                    gridTiles[colIndex, i] = go.GetComponent<Tile>();
                    gridTiles[colIndex, i].SetVisualForced(gridTiles[colIndex, i].TileID);
                    //Destroy top tile
                    Destroy(columnFillTiles[i].gameObject);
                }
                columnFillTiles.Clear();
            }

            //Clear old completed link list
            for (int i = 0; i < completedLink.Count; i++)
            {
                Destroy(completedLink[i].gameObject);
            }
            completedLink.Clear();
        }

        if(OnFillCompleted != null)
            OnFillCompleted();

        yield return 0;
    }
}
