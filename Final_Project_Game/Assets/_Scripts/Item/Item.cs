using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;

namespace Game
{
    public class Item : MonoBehaviour, IInteractable, IPickupable
    {
        [SerializeField] private Sprite _icon;

        public void Interact(object interactor)
        {

        }

        public void Pickup(Inventory inventory)
        {
            inventory.AddToInventory(this);
        }

        public Sprite GetIcon()
        {
            return _icon;
        }
    }
    
}
