using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;


[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public bool stackable;
    public Sprite icon;
    public ToolAction onAction;

    //[SerializeField] protected Sprite _icon;
    //public void Interact(object interactor)
    //{

    //}

    //public virtual void Pickup(Inventory inventory)
    //{
    //    inventory.AddToInventory(this);
    //}

    //public virtual Sprite GetIcon()
    //{
    //    return _icon;
    //}
}
   
