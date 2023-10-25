using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ToolAction/Place Object")]
public class PlaceObject : ToolAction
{
    public override bool OnApplyTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {

        tileMapReadController.objectsManager.PlaceObject(item, gridPosition);
        return true;
    }
    
}
