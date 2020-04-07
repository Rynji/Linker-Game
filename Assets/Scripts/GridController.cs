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

    
    void Start()
    {
        linkController = this.GetComponent<LinkController>();
        columnFillTiles = new List<Tile>();

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
        int emptySpotsInCol = 0;

        //Collapse down existing blocks on empty spots
        for (int j = rows; j --> 0;) //reversed loop as I want to work from the bottom up.
        {
            if (gridTiles[0, j] == null)
            {
                print("Empty spot found at: " + 0 + "," + j);
                emptySpotsInCol++;
                
                //First empty spot encountered, this empty spot needs to grab a block from above before moving on.
                //So first thing: we find the nearest not empty block and place it at our empty position.
                
                //if(Block above not empty)
                {
                    //Move not empty block to empty position
                }
                //else -> loop up until non empty block found or < 0 reached
                //non-empty block found: move it to empty position
                //< 0 reached: Grab first block from columnFillTiles[] under here.

            }
        }

        //Spawn new Blocks
        for (int i = 0; i < emptySpotsInCol; i++)
        {
            GameObject go = Instantiate(tilePrefab, GetGridPosition(0, -i -1), Quaternion.identity, this.gameObject.transform);
            columnFillTiles.Add(go.GetComponent<Tile>());
            columnFillTiles[i].SetVisual();
        }

        //Collapse down new blocks on empty spots

        

        if(OnFillCompleted != null)
            OnFillCompleted();

        yield return 0;
    }
}
