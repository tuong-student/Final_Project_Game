using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum StorageType
{
    None,
    FarmItem,
    Crop,
    Weapon,
    Object,
}

public abstract class Storable : ScriptableObject
{
    public abstract bool Tradable{get;}
    public abstract bool Stackable{get;}
    public abstract Sprite IconImage{get;}
    public abstract StorageType StorageType{get;}
    public abstract int Id{get;}
    public abstract int Price{get;}
}
