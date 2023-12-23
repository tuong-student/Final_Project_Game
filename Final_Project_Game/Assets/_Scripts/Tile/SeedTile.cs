using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/ToolAction/Seed Tile")]
public class SeedTile : ToolAction
{
    public override bool OnApplyTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Storable storable)
    {
        CropSO cropSO = storable as CropSO;
        if (!tileMapReadController.cropsManager.Check(gridPosition)) 
            return false;
        tileMapReadController.cropsManager.Seed(gridPosition, cropSO.crop);
        PlayerManager.Instance.RemoveFromInventory(storable, 1);
        return true;
    }
    public override void OnItemUsed(Storable usedItem, ItemContainer inventory)
    {
        inventory.Remove(usedItem);
    }
}
