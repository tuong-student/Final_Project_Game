using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Data/ToolAction/Plow")]
public class PlowTile : ToolAction
{
    [SerializeField] List<TileBase> canPlow;

    //TODO: Audio here;

    public override bool OnApplyTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
        TileBase tileToPlow = tileMapReadController.GetTileBase(gridPosition);
        if (!canPlow.Contains(tileToPlow))
        {
            return false;
        }
        tileMapReadController.cropsManager.Plow(gridPosition);
        return true;
    }
}
