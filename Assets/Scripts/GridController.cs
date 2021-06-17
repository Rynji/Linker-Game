using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LinkController))]
public class GridController : MonoBehaviour
{
    public event Action OnFillCompleted, OnGridShuffle;

    [Header("Gameplay Values")]
    [Range(5, 16)]
    [SerializeField] private int rows;
    [Range(5, 16)]
    [SerializeField] private int cols;
    [SerializeField] private float tileSize;
    //[Header("Debug Tools")]
    //[SerializeField] private bool setGridUpdate;

    private Tile[,] gridTiles;
    private List<Tile> completedLink;
    private List<Tile> columnFillTiles;
    private LinkController linkController;
    private GameObject tilePrefab;
    private Transform gridTransform;
    private Bounds gridBounds;
    private bool isGridActive;

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
    public GameObject TilePrefab { get => tilePrefab; set => tilePrefab = value; }

    
    void Start()
    {
        linkController = this.GetComponent<LinkController>();
        
        columnFillTiles = new List<Tile>();
        completedLink = new List<Tile>();
        gridTiles = new Tile[cols, rows];

        gridTransform = this.gameObject.transform;

        //Center the grid based on tileSize/spacing
        gridTransform.position = new Vector3((-tileSize * cols / 2f) + tileSize * 0.5f, (tileSize * rows / 2f) - tileSize * 0.5f, 0f);
        ScaleCameraToGrid();
    }

    void Update()
    {
        //Used at the beginning of development to determine grid size/positioning.
        // if (setGridUpdate)
        // {
        //     for (int i = 0; i < rows; i++)
        //     {
        //         for (int j = 0; j < cols; j++)
        //         {
        //             gridTiles[i, j].transform.position = GetGridPosition(i, j);
        //         }
        //     }
        // }
    
        if(isGridActive && Input.GetKeyDown(KeyCode.R))
        {
            ClearGrid();
            FillGrid();
        }
    }


    public void FillGrid()
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

        CheckGridForLinks();
    }

    public void ResetGridController()
    {
        ClearGrid();
        columnFillTiles = new List<Tile>();
        completedLink = new List<Tile>();
        gridTiles = new Tile[cols, rows];
    }

    public void ScaleCameraToGrid()
    {
        //Create bounds around the grid, *1.4f in the height to accomodate the gameplay UI
        gridBounds = new Bounds(new Vector3(0, 0, 0), new Vector3(cols * tileSize, (rows * tileSize) * 1.4f, 1));
        //Scale camera according to the grid bounds
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = gridBounds.size.x / gridBounds.size.y;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = gridBounds.size.y / 2; //Scale camera to grid width
        }
        else 
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = gridBounds.size.y / 2 * differenceInSize; //Add the required extra height to the orthographic size through differenceInSize
        }
    }

    private void ClearGrid()
    {
        isGridActive = false;

        //Since the grid can be changed during runtime just clear all the tiles manually instead of going by rows/cols.
        for (int i = transform.childCount; i --> 0;)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }

        Array.Clear(gridTiles, 0, gridTiles.Length);
    }

    /// <summary>
    /// Reshuffles the grid if there are no valid links to be made.
    /// </summary>
    private void CheckGridForLinks()
    {
        if(!linkController.HasGridValidLink())
        {
            if(OnGridShuffle != null)
                OnGridShuffle();

            ClearGrid();
            FillGrid();
        }
        isGridActive = true;
    }

    private Vector3 GetGridPosition(int i, int j)
    {
        Vector3 gridPos = new Vector3();

        gridPos = this.gameObject.transform.localPosition + new Vector3((i * tileSize), (-j * tileSize), 0f);

        return gridPos;
    }

    public IEnumerator RefillGrid()
    {
        isGridActive = false;

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
                    lookingForBlock = true;
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
                yield return new WaitForSeconds(0.0025f);
            }

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
                    yield return new WaitForSeconds(0.001f);
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

        CheckGridForLinks();

        yield return 0;
    }
}
