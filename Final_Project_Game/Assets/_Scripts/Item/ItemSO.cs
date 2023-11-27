using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;


[CreateAssetMenu(menuName = "Data/Item")]
[System.Serializable]
public class ItemSO : Storable
{
    public string Name;
    public int id;
    public int price;
    public bool stackable;
    public bool tradeable;
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
    public override int Price => price;
    public override bool Tradable => tradeable;
}
   
