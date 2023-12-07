using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;


[CreateAssetMenu(menuName = "Data/Item")]
[System.Serializable]
public class ItemSO : Storable, IHoldableItem
{
    #region original parameter
    public Sprite icon;
    public ToolAction onAction;
    public ToolAction onTileMapAction;
    public ToolAction onItemUsed;
    public Crop crop;
    public GameObject itemPrefab;
    public RuntimeAnimatorController animator;
    public string Name;
    public int id;
    public int price;
    public bool stackable;
    public bool tradeable;
    public bool iconHighlight;
    public float interactRate;
    #endregion

    #region Interface parameter
    public override bool Stackable => stackable;
    public override Sprite Icon => icon;
    public override int Id => id;
    public override StorageType StorageType => StorageType.FarmItem;
    public override int Price => price;
    public override bool Tradable => tradeable;
    public float InteractRate => interactRate;
    public Sprite HoldImage => icon;
    public Storable StorableData => this;
    #endregion
}
   
