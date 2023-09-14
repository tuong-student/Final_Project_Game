using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapReadController : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] List<TileData> tileData;
    Dictionary<TileBase, TileData> dataFromTiles;

    private void Start()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach(TileData tileData in tileData)
        {
            foreach (TileBase tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    public Vector3Int GetGridPosition(Vector2 position, bool mousePosition)
    {
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
        gridPosition.z = 0;
        TileBase tile = tilemap.GetTile(gridPosition);


        Debug.Log("Tile in position = " + gridPosition + " os " + tile);
        return tile;
    }

    public TileData GetTileData(TileBase tilebase)
    {
        return dataFromTiles[tilebase];
    }
}
