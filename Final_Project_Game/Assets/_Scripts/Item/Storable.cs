using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum StorageType
{
    FarmItem,
    Weapon
}

public abstract class Storable : ScriptableObject
{
    public abstract bool Stackable{get;}
    public abstract Sprite Icon{get;}
    public abstract StorageType StorageType{get;}
    public abstract int Id{get;}
}
