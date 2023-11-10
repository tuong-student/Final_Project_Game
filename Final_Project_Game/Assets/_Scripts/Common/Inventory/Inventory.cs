using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Interface;
using Newtonsoft.Json;

namespace Game
{
    [Serializable]
    public class InventoryStack 
    {
        public Item _item;
        public int _quantity;
        public int _maxQuantity = 20;
        public InventoryStack(Item item, int quantity = 1)
        {
            this._item = item;
            this._quantity = quantity;
        }
        public void StackAndReturn(int quantity, out int returnQuantity)
        {
            int lessQuantity = _maxQuantity - _quantity;
            _quantity = Math.Clamp(_quantity + quantity, 0, _maxQuantity);
            returnQuantity = Math.Clamp(quantity - lessQuantity, 0, quantity);
        }
        public bool IsStackable()
        {
            return _maxQuantity > _quantity;
        }
    }

    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<InventoryStack> _inventory = new List<InventoryStack>();

        public List<InventoryStack> GetInventoryStack()
        {
            return _inventory;
        }

        public void AddToInventory(Item item)
        {
            if(TryToGetInventoryStackables(item, out List<InventoryStack> stackableList))
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
                _inventory.Add(new InventoryStack(item));
            }
        }
        public void AddToInventory(ItemStack itemStack)
        {
            AddToInventory(itemStack.GetItem(), itemStack.GetQuantity());
        }
        public void AddToInventory(Item item, int quantity)
        {
            if(TryToGetInventoryStackables(item, out List<InventoryStack> stackableList))
            {
                int returnQuantity = quantity;
                foreach(var stackable in stackableList)
                {
                    stackable.StackAndReturn(returnQuantity, out returnQuantity);
                }
                if(returnQuantity > 0)
                {
                    AddToInventory(item, returnQuantity);
                }
            }
            else
            {
                _inventory.Add(new InventoryStack(item, quantity));
            }
        }

        public void RemoveFromInventory(InventoryStack stack)
        {
            _inventory.Remove(stack);
        }

        public bool TryToGetInventoryStackables(Item item, out List<InventoryStack> stacks)
        {
            stacks = null;
            List<InventoryStack> tempItemStack = new List<InventoryStack>();
            tempItemStack = _inventory.Where(x => x._item == item).ToList();
            if(!tempItemStack.Select(x => x._item).Contains(item) || !tempItemStack.Exists(x => x.IsStackable() == true))
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
            return _inventory[index]._item;
        }
    }
}
