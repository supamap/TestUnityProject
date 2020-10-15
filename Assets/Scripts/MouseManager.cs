using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseManager : MonoBehaviour
{
    [SerializeField] public Tilemap groundTilemap, overlayTilemap;
    [SerializeField] Tile selectorTile;

    UnitScript selectedUnit;

    public Vector3Int selected_tile;

    public Dictionary<Vector3Int, WorldTile> worldTiles;



    // Start is called before the first frame update
    void Start()
    {
        worldTiles = new Dictionary<Vector3Int, WorldTile>();
        foreach (var pos in groundTilemap.cellBounds.allPositionsWithin)
        {
            TileBase currentTile = groundTilemap.GetTile(pos);
            if (currentTile)
            {
                WorldTile currentWorldTile = new WorldTile(this, pos);
                worldTiles.Add(pos,currentWorldTile);
            }
            
        }

        foreach (var wt in worldTiles.Values)
        {
            wt.GenerateNeighbors();
        }

        Debug.Log(worldTiles.Count + " TILES CREATED");
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
                    SelectUnit(unit);
                }
                else
                {
                    DeselectUnit();
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
            Debug.Log("MOVE TO TILE");
            if (selectedUnit)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cell_pos = groundTilemap.WorldToCell(pos);

                
                if (worldTiles.ContainsKey(cell_pos))
                {
                    WorldTile targetTile = worldTiles[cell_pos];
                    if (targetTile.IsEmpty())
                    {
                        TileBase clickedTile = groundTilemap.GetTile(cell_pos);
                        //move the selected object to the tile
                        selectedUnit.MoveToTile(cell_pos);
                    }
                    else
                    {
                        Debug.Log("Tile is full");
                    }

                }
            }
            


        }
    }


    private void OnMouseDown()
    {
        Debug.Log("MANAGER CLICKED");
    }

    public void DeselectUnit()
    {
        if (selectedUnit)
        {
            selectedUnit.Unselect();
        }
    }
    public void SelectUnit(UnitScript unit)
    {
        Debug.Log(unit.name + " SELECTED");
        DeselectUnit();

        unit.Select();
        selectedUnit = unit;

        if (unit.currentWorldTile != null)
        {
            //Debug.Log(unit.currentWorldTile.coordinates);
            foreach (var t in unit.currentWorldTile.neighbors)
            {
                //Debug.Log("NEIGHBOR: " + t);
            }
            HashSet<Vector3Int> twd = unit.currentWorldTile.GetTilesWithinDistance(2.1f);
            Debug.Log(twd.Count);
            foreach (var t in twd)
            {
                Debug.Log(t);
            }
        }

    }
}
