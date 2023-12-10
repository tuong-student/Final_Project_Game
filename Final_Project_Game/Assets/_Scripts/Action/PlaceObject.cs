using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ToolAction/Place Object")]
public class PlaceObject : ToolAction
{
    public override bool OnApplyTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Storable storable)
    {
        ItemSO itemSO = storable as ItemSO;
        if (tileMapReadController.objectsManager.CheckPosition(gridPosition))
            return false;
        tileMapReadController.objectsManager.PlaceObject(itemSO, gridPosition);
        return true;
    }

    public override void OnItemUsed(Storable usedItem, ItemContainer inventory)
    {
        inventory.Remove(usedItem);
    }
}
