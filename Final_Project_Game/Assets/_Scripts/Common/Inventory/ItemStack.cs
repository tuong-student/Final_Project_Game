using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ItemStack : Item
    {
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private TMPro.TextMeshPro _itemNumber;
        private Item _item;
        private int _quantity;

        public static ItemStack Create(Transform parent = null)
        {
            return Instantiate(Resources.Load<ItemStack>("Resources/Prefabs/ItemStack"), parent);
        }

        public void SetItemAndQuantity(Item item, int quantity)
        {
            _item = item;
            _icon = item.GetIcon();
            _quantity = quantity;
            _sr.sprite = item.GetIcon();
            _itemNumber.text = quantity.ToString();
        }

        public override void Pickup(Inventory inventory)
        {
            inventory.AddToInventory(this);
        }

        public Item GetItem()
        {
            return _item;
        }
        public int GetQuantity()
        {
            return _quantity;
        }
        public InventoryStack ToInventoryStack()
        {
            return new InventoryStack(_item, _quantity);
        }
    }

}
