using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private GridController grid;
    [SerializeField] private LinkController linkController;

    private List<GameObject> selectedTiles;
    private int hitCounter;
    private bool lockInput;

    
    void Start()
    {
        selectedTiles = new List<GameObject>();
        grid.OnFillCompleted += () => { lockInput = false; };
    }

    void Update()
    {
        if (!lockInput && Input.GetMouseButton(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                //Debug.Log("Hit " + hitInfo.transform.gameObject.name);

                if(selectedTiles.Count == 0) //First hit
                {
                    selectedTiles.Add(hitInfo.transform.gameObject);
                    selectedTiles[selectedTiles.Count - 1].GetComponent<Tile>().ToggleLinkVisual(true);
                }
                else if(CheckSelection(hitInfo))
                {
                    selectedTiles.Add(hitInfo.transform.gameObject);
                    selectedTiles[selectedTiles.Count - 1].GetComponent<Tile>().ToggleLinkVisual(true);
                }
            }
            else
            {
                //Debug.Log("No hit");
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //print("Mouse Released");
            FinishSelectedLink();
        }
    }

    private bool CheckSelection(RaycastHit hitInfo)
    {
        //If selected tile is not yet added to selected tiles; proceed.
        if(!selectedTiles.Contains(hitInfo.transform.gameObject)) 
        {
            //If selected tile is of the same type as the previously selected tile & a neighbour; proceed.
            if(linkController.IsTileMatchingNeighbour(hitInfo.transform.gameObject.GetComponent<Tile>(), selectedTiles[selectedTiles.Count - 1].GetComponent<Tile>()))
            {
                return true;
            }
        }
        else //Tile already in the list, maybe the player wants to remove his latest link, check if hit tile is previous hit tile.
        {
            if(selectedTiles.Count > 1 && selectedTiles[selectedTiles.Count - 2].Equals(hitInfo.transform.gameObject))
            {
                //print("Previous tile hit!");
                selectedTiles[selectedTiles.Count - 1].GetComponent<Tile>().ToggleLinkVisual(false);
                selectedTiles.RemoveAt(selectedTiles.Count - 1); //Remove latest hit tile, because the player swiped back to previous tile on his/her link.
            }
        }

        return false;
    }

    private void ResetSelection()
    {
        hitCounter = 0;
            for (int i = 0; i < selectedTiles.Count; i++)
            {
                selectedTiles[i].GetComponent<Tile>().ToggleLinkVisual(false);
            }
            selectedTiles.Clear();
    }

    private void FinishSelectedLink()
    {
        //Check if selected link is big enough then walk through the whole grid and set all selected tiles as completed.
        if(selectedTiles.Count > linkController.TilesRequiredForLink - 1)
        {
            for (int i = 0; i < grid.Cols; i++)
            {
                for (int j = 0; j < grid.Rows; j++)
                {
                    for (int k = 0; k < selectedTiles.Count; k++)
                    {
                        if (grid.GridTiles[i, j] != null && grid.GridTiles[i, j].gameObject.Equals(selectedTiles[k]))
                        {
                            //Destroy(grid.GridTiles[i, j].gameObject);
                            //grid.GridTiles[i,j] = null;
                            //Don't destroy the tiles, set them as completed as we need their information when moving them down.
                            grid.GridTiles[i, j].IsCompleted = true;
                            grid.GridTiles[i, j].GetComponent<SpriteRenderer>().sprite = null;
                            grid.CompletedLink.Add(grid.GridTiles[i, j]);
                            break;
                        }
                    }
                }
            }

            //Reset
            ResetSelection();

            //Lock input until fill has been completed
            lockInput = true;

            //TODO: Event OnLinkSuccesfull

            StartCoroutine(grid.RefillGrid());
        }
        else //Link not big enough
        {
            ResetSelection();
        }
    }
}
