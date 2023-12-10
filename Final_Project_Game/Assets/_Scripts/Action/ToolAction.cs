using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolAction : ScriptableObject
{
    public virtual bool OnApply(Vector2 worldPoint)
    {
        Debug.LogWarning("not implemented");
        return true;
    }
    public virtual bool OnApplyTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Storable cropSO)
    {
        Debug.LogWarning("OnApplyToTileMap is not implemented");
        return true;
    }
    public virtual void OnItemUsed(Storable usedItem, ItemContainer inventory)
    {

    }
}
