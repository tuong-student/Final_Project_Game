using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapReadController : MonoBehaviour
{

    [SerializeField] Tilemap tilemap;
    public CropsManager cropsManager;
    public PlaceableObjectReferenceManager objectsManager;

    public Vector3Int GetGridPosition(Vector2 position, bool mousePosition)
    {
        if (tilemap == null)
            tilemap = GameObject.Find("BaseTilemap").GetComponent<Tilemap>();
        if (tilemap == null)
            return Vector3Int.zero;
        Vector3 worldPosition;
        if (mousePosition)
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        else
            worldPosition = position;
        Vector3Int gridPosition = tilemap.WorldToCell(worldPosition);
        return gridPosition;

    }

    public TileBase GetTileBase(Vector3Int gridPosition)
    {
        if (tilemap == null)
            tilemap = GameObject.Find("BaseTilemap").GetComponent<Tilemap>();
        if (tilemap == null)
            return null;
        gridPosition.z = 0;
        TileBase tile = tilemap.GetTile(gridPosition);


        Debug.Log("Tile in position = " + gridPosition + " os " + tile);
        return tile;
    }

}
