using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/ToolAction/Harvest")]
public class TilePickUpAction : ToolAction
{
    public override bool OnApplyTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Storable storable)
    {
        tileMapReadController.cropsManager.PickUp(gridPosition);
        tileMapReadController.objectsManager.PickUp(gridPosition);
        return true;
    }
}
