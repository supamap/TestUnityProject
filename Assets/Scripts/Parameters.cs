using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine.Tilemaps;

public static class Parameters
{
    public static Dictionary<string, float> terrainMovement = new Dictionary<string, float>()
    {
        { "grass",1f },
        { "sand", 2f },
        { "wall", Mathf.Infinity }
    };

    public static Dictionary<Tile, string> terrainTiles = new Dictionary<Tile, string>()
    {
        {AssetDatabase.LoadAssetAtPath<Tile>("Assets/Sprites/Tile_Grass.asset"),"grass" },
        {AssetDatabase.LoadAssetAtPath<Tile>("Assets/Sprites/Tile_Sand.asset"),"wall" },
    };
}
