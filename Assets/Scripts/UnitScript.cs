using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    GameObject gameManager;
    MouseManager manager;

    bool selected;
    bool movable;
    SpriteRenderer childRenderer;

    Color unselectedColor = new Color(1f, 1f, 1f);
    Color unmovableColor = new Color(0.5f, 0.5f, 0.5f);
    Color selectedColor = new Color(1f, 0.5f, 0.5f);

    public WorldTile currentWorldTile;

    float movementPoints;

    public HashSet<Vector3Int> movableTiles;



    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        Debug.Log(gameManager);
        manager = gameManager.GetComponent<MouseManager>();
        selected = false;
        movable = true;
        childRenderer = GetComponentInChildren<SpriteRenderer>();

        movementPoints = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            childRenderer.color = selectedColor;
        }
        else
        {
            if (movable)
            {
                childRenderer.color = unselectedColor;
            }
            else
            {
                childRenderer.color = unmovableColor;
            }
        }

    }

    public void Select()
    {
        selected = true;
    }

    public void Unselect()
    {
        selected = false;
    }

    public void MoveToTile(Vector3Int targetTile)
    {
        if (movable) {
            WorldTile targetWorldTile = manager.worldTiles[targetTile];

            if (currentWorldTile != null)
            {
                currentWorldTile.SetUnit(null);
            }
            currentWorldTile = targetWorldTile;
            transform.position = targetWorldTile.worldPosition;
            targetWorldTile.SetUnit(this);

            movable = false;
        }


    }

    public void RefreshMovableTiles()
    {
        movableTiles = currentWorldTile.GetTilesWithinDistance(movementPoints);

    }

    public void NextTurn()
    {
        movable = true;
    }

}
