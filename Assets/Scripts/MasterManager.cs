using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MasterManager : MonoBehaviour
{
    MouseManager mouseManager;
    OverlayManager overlayManager;

    [SerializeField] public Tilemap groundTilemap, overlayTilemap;

    public Dictionary<Vector3Int, WorldTile> worldTiles;

    internal UnitScript selectedUnit;


    // Start is called before the first frame update
    void Start()
    {
        mouseManager = GetComponent<MouseManager>();
        overlayManager = GetComponent<OverlayManager>();


        worldTiles = new Dictionary<Vector3Int, WorldTile>();
        foreach (var pos in groundTilemap.cellBounds.allPositionsWithin)
        {
            TileBase currentTile = groundTilemap.GetTile(pos);
            if (currentTile)
            {
                WorldTile currentWorldTile = new WorldTile(this, pos);
                worldTiles.Add(pos, currentWorldTile);
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
        if (Input.GetKeyDown("space"))
        {
            NextTurn();
        }
    }

    public void MoveUnit(UnitScript movingUnit, Vector3Int cell_pos)
    {
        WorldTile targetTile = worldTiles[cell_pos];
        bool reachable;
        if (movingUnit.currentWorldTile is null)
        {
            reachable = true;
        }
        else
        {
            reachable = movingUnit.movableTiles.Contains(cell_pos);
        }

        if (reachable)
        {
            if (targetTile.IsEmpty())
            {
                Debug.Log("MOVE TO TILE");
                //move the selected object to the tile
                movingUnit.MoveToTile(cell_pos);
                DeselectUnit();
            }
            else
            {
                Debug.Log("Tile is full");
            }
        }
        else
        {
            Debug.Log("Tile not reachable");
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
            // update movable tiles
            unit.RefreshMovableTiles();
            // show movable tiles
            overlayManager.ShowMovableTiles(unit.movableTiles);

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

    public void DeselectUnit()
    {
        if (selectedUnit)
        {
            selectedUnit.Unselect();
        }
        overlayManager.ClearOverlayTiles();
    }

    public void PlaceUnit()
    {
        //instantiate the unit, then put it in a tile

    }

    public void NextTurn()
    {
        UnitScript[] units = GameObject.FindObjectsOfType<UnitScript>();
        foreach (var unit in units)
        {
            unit.NextTurn();
        }
    }
}
