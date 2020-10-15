using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OverlayManager : MonoBehaviour
{

    MasterManager masterManager;

    public Dictionary<Vector3Int, WorldTile> worldTiles;

    Tilemap overlayTilemap;

    [SerializeField] Tile movableTileSprite;

    // Start is called before the first frame update
    void Start()
    {
        masterManager = GetComponent<MasterManager>();
        overlayTilemap = masterManager.overlayTilemap;
        worldTiles = masterManager.worldTiles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMovableTiles(HashSet<Vector3Int> movableTiles)
    {
        ClearOverlayTiles();

        Debug.Log("SHOWING MOVABLE TILES");
        foreach (var coord in movableTiles)
        {
            overlayTilemap.SetTile(coord, movableTileSprite);
        }
    }

    public void ClearOverlayTiles()
    {
        foreach (var coord in masterManager.worldTiles.Keys)
        {
            overlayTilemap.SetTile(coord, null);
        }
    }
}
