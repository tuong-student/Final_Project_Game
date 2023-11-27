using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlot
{
    public Storable storable;
    public int count;

    public void Copy(ItemSlot slot)
    {
        storable = slot.storable;
        count = slot.count;
    }

    public void Set(Storable item, int count)
    {
        this.storable = item;
        this.count = count;
    }
    public void Clear()
    {
        storable = null;
        count = 0;
    }
}
[CreateAssetMenu(menuName = "Data/Item Container")]
public class ItemContainer : ScriptableObject
{
    public List<ItemSlot> slots;
    public bool isDirty;

    public void Init()
    {
        slots = new List<ItemSlot>();
        for (int i = 0; i < 36; i++)
        {
            slots.Add(new ItemSlot());
        }
    }

    public void Add(Storable item, int count = 1)
    {
        isDirty = true;
        if (item.Stackable)
        {
            ItemSlot itemSlot = slots.Find(x => x.storable == item);
            if (itemSlot != null)
            {
                itemSlot.count += count;
            }
            else
            {
                itemSlot = slots.Find(x => x.storable == null);
                if(itemSlot != null)
                {
                    itemSlot.storable = item;
                    itemSlot.count = count;
                }
            }
        }
        else
        {
            //add non stackable item to ours item container
            ItemSlot itemSlot = slots.Find(x => x.storable == null);
            if (itemSlot != null)
            {
                itemSlot.storable = item;
            }
        }
    }
    public void Remove(Storable itemToRemove, int count = 1)
    {
        isDirty = true;
        if (itemToRemove.Stackable)
        {
            ItemSlot itemSlot = slots.Find(x => x.storable == itemToRemove);
            if (itemSlot == null) return;
            itemSlot.count -= count;
            if (itemSlot.count <= 0)
                itemSlot.Clear();
        }
        else
        {
            while (count > 0)
            {
                count -= 1;
                ItemSlot itemSlot = slots.Find(x => x.storable == itemToRemove);
                if (itemSlot == null) break;

                itemSlot.Clear();
            }
        }
    }

    public bool CheckFreeSpace()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].storable == null)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckItem(ItemSlot checkingItem)
    {
        ItemSlot itemSlot = slots.Find(x => x.storable == checkingItem.storable);
        if (itemSlot == null) 
            return false;
        if (checkingItem.storable.Stackable)
            return itemSlot.count >= checkingItem.count;
        return true;
    }
}
