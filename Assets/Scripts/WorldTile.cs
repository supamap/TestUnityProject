using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile
{
    MasterManager manager;
    public Vector3Int coordinates;
    Tile terrain;
    UnitScript unit;
    public Vector3 worldPosition;
    float movementCost;
    public List<Vector3Int> neighbors;


    public WorldTile(MasterManager _manager, Vector3Int _coordinates)
    {
        manager = _manager;
        coordinates = _coordinates;
        terrain = (Tile)manager.groundTilemap.GetTile(coordinates);
        worldPosition = manager.groundTilemap.CellToWorld(coordinates);


        movementCost = Parameters.terrainMovement[Parameters.terrainTiles[terrain]];
        
    }

    public bool IsEmpty()
    {
        if (unit)
        {
            return false;
        }
        return true;
    }

    public void SetUnit(UnitScript new_unit)
    {
        unit = new_unit;
    }


    public void GenerateNeighbors()
    {
        List<Vector3Int> _neighbors = new List<Vector3Int>();
        List<Vector3Int> possibleOffsets = new List<Vector3Int>()
        {
            new Vector3Int(1,0,0),
            new Vector3Int(-1,0,0),
            new Vector3Int(0,1,0),
            new Vector3Int(0,-1,0)
        };
        foreach (var offset in possibleOffsets)
        {
            Vector3Int targetCoords = offset + coordinates;
            if (manager.worldTiles.ContainsKey(targetCoords))
            {
                _neighbors.Add(targetCoords);
            }
        }

        neighbors = _neighbors;
    }


    public HashSet<Vector3Int> GetTilesWithinDistance(float maxD)
    {
        Dictionary<Vector3Int, float> dist = new Dictionary<Vector3Int, float>();
        HashSet<Vector3Int> reached = new HashSet<Vector3Int>();

        HashSet<Vector3Int> toVisit = new HashSet<Vector3Int>();

        foreach (Vector3Int coord in manager.worldTiles.Keys)
        {
            dist.Add(coord, Mathf.Infinity);
        }
        dist[coordinates] = 0;
        toVisit.Add(coordinates);

        while (toVisit.Count > 0)
        {
            //from the tiles in toVisit, get the one with the smallest distance

            Vector3Int closest = toVisit.OrderBy(x => dist[x]).First();
            Debug.Log(closest);
            float closest_dist = dist[closest];

            //find all its neighbors
            List<Vector3Int> close_n = manager.worldTiles[closest].neighbors;
            foreach (var n in close_n)
            {
                //check if neighbors are not visited
                if (!reached.Contains(n))
                {
                    // calculate the distance to the n
                    float n_cost = manager.worldTiles[n].movementCost; 
                    float n_dist = closest_dist + n_cost;
                    if ((dist[n] > n_dist) && (n_dist <= maxD))
                    {
                        toVisit.Add(n);
                        dist[n] = n_dist;
                    }
                }
            }
            reached.Add(closest);
            toVisit.Remove(closest);
        }

        return reached;
    }
}
