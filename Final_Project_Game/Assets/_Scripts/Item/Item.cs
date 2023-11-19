using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;


[CreateAssetMenu(menuName = "Data/Item")]
[System.Serializable]
public class Item : Storable
{
    public string Name;
    public int id;
    public bool stackable;
    public Sprite icon;
    public ToolAction onAction;
    public ToolAction onTileMapAction;
    public ToolAction onItemUsed;
    public Crop crop;
    public bool iconHightlight;
    public GameObject itemPrefab;

    public override bool Stackable => stackable;
    public override Sprite Icon => icon;
    public override int Id => id;
    public override StorageType StorageType => StorageType.FarmItem;
}
   
