using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseManager : MonoBehaviour
{

    MasterManager masterManager;

    public Dictionary<Vector3Int, WorldTile> worldTiles;

    Tilemap groundTilemap, overlayTilemap;


    // Start is called before the first frame update
    void Start()
    {
        masterManager = GetComponent<MasterManager>();
        groundTilemap = masterManager.groundTilemap;

        worldTiles = masterManager.worldTiles;
    }

    // Update is called once per frame
    void Update()
    {

        //If the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            Debug.Log(worldPoint);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider)
            {
                UnitScript unit = hit.collider.GetComponentInParent<UnitScript>();
                if (unit)
                {
                    masterManager.SelectUnit(unit);
                }
                else
                {
                    masterManager.DeselectUnit();
                }

            }
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    //Debug.Log("GRID CLICKED");
        //    //Debug.Log("GRID CLICKED");
        //    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector3Int cell_pos = groundTilemap.WorldToCell(pos);
        //    TileBase tile = groundTilemap.GetTile(cell_pos);
        //    if (tile)
        //    {
        //        overlayTilemap.SetTile(selected_tile, null);
        //        selected_tile = cell_pos;
        //        overlayTilemap.SetTile(selected_tile, selectorTile);
        //    }
        //}

        if (Input.GetMouseButtonDown(1))
        {
            if (masterManager.selectedUnit)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cell_pos = groundTilemap.WorldToCell(pos);
                
                if (worldTiles.ContainsKey(cell_pos))
                {
                    masterManager.MoveUnit(masterManager.selectedUnit, cell_pos);
                }
            }
        }
    }


}
