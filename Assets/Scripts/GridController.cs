using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
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


    void Start()
    {
        gridTiles = new Tile[rows, cols];

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
    
        
    }

    private void FillGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject go = Instantiate(tilePrefab, GetGridPosition(i, j), Quaternion.identity, this.gameObject.transform);
                go.name = "Tile_" + i + "_" + j;
                gridTiles[i, j] = go.GetComponent<Tile>();
                gridTiles[i, j].SetVisual();
            }
        }
    }

    private Vector3 GetGridPosition(int i, int j)
    {
        Vector3 gridPos = new Vector3();

        gridPos = this.gameObject.transform.localPosition + new Vector3((i * tileSize), (-j * tileSize), 0f);

        return gridPos;
    }
}
