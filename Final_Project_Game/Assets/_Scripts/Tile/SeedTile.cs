using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/ToolAction/Seed Tile")]
public class SeedTile : ToolAction
{
    public override bool OnApplyTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, ItemSO item)
    {
        if (!tileMapReadController.cropsManager.Check(gridPosition)) 
            return false;
        tileMapReadController.cropsManager.Seed(gridPosition,item.crop);
        return true;
    }
    public override void OnItemUsed(ItemSO usedItem, ItemContainer inventory)
    {
        inventory.Remove(usedItem);
    }
}
