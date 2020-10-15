using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    GameObject gameManager;
    MouseManager manager;

    bool selected;
    SpriteRenderer childRenderer;

    Color unselectedColor = new Color(1f, 1f, 1f);
    Color selectedColor = new Color(1f, 0.5f, 0.5f);

    public WorldTile currentWorldTile;

    float movementPoints;



    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        Debug.Log(gameManager);
        manager = gameManager.GetComponent<MouseManager>();
        selected = false;
        childRenderer = GetComponentInChildren<SpriteRenderer>();

        movementPoints = 2f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Select()
    {
        selected = true;
        childRenderer.color = selectedColor;
    }

    public void Unselect()
    {
        selected = false;
        childRenderer.color = unselectedColor;
    }

    public void MoveToTile(Vector3Int targetTile)
    {
        WorldTile targetWorldTile = manager.worldTiles[targetTile];

        if (currentWorldTile != null)
        {
            currentWorldTile.SetUnit(null);
        }
        currentWorldTile = targetWorldTile;
        transform.position = targetWorldTile.worldPosition;
        targetWorldTile.SetUnit(this);

    }
}
