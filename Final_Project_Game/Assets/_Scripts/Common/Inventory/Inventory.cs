using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Interface;

namespace Game
{
    [Serializable]
    public class ItemStack 
    {
        public Item item;
        public int quantity;
        public int maxQuantity = 20;
        public ItemStack(Item item, int quantity = 1)
        {
            this.item = item;
            this.quantity = quantity;
        }
        public void StackAndReturn(int quantity, out int returnQuantity)
        {
            returnQuantity = 0;
            if(quantity == 0) return;

            int lessQuantity = maxQuantity - quantity;
            returnQuantity = Math.Clamp(quantity - lessQuantity, 0, 20);
            this.quantity += quantity - returnQuantity;
        }
        public bool IsStackable()
        {
            return maxQuantity > quantity;
        }
    }

    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<ItemStack> _inventory = new List<ItemStack>();

        public List<ItemStack> GetItemStacks()
        {
            return _inventory;
        }

        public void AddToInventory(Item item)
        {
            if(TryToGetItemStackables(item, out List<ItemStack> stackableList))
            {
                foreach(var stackable in stackableList)
                {
                    // After stack to the first item stack, return
                    stackable.StackAndReturn(1, out int returnValue);
                    return;
                }
            }
            else
            {
                _inventory.Add(new ItemStack(item));
            }
        }
        public void AddToInventory(Item item, int quantity)
        {
            if(TryToGetItemStackables(item, out List<ItemStack> stackableList))
            {
                int lessQuantity = quantity;
                foreach(var stackable in stackableList)
                {
                    stackable.StackAndReturn(lessQuantity, out lessQuantity);
                }
            }
            else
            {
                _inventory.Add(new ItemStack(item, quantity));
            }
        }

        public bool TryToGetItemStackables(Item item, out List<ItemStack> stacks)
        {
            stacks = null;
            List<ItemStack> tempItemStack = new List<ItemStack>();
            tempItemStack = _inventory.Where(x => x.item == item).ToList();
            if(!tempItemStack.Select(x => x.item).Contains(item) || !tempItemStack.Exists(x => x.IsStackable() == true))
            {
                return false;
            }
            else
            {
                stacks = tempItemStack.Where(x => x.IsStackable() == true).ToList();
                return true;
            }
        }

        public Item GetItemBaseOnIndex(int index)
        {
            return _inventory[index].item;
        }
    }
}
