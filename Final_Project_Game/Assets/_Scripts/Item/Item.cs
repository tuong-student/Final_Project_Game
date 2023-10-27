using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;


[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
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
}
   
