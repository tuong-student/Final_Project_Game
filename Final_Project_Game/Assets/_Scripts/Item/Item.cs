using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;

namespace Game
{
    public class Item : MonoBehaviour, IInteractable, IPickupable
    {
        [SerializeField] protected Sprite _icon;

        public void Interact(object interactor)
        {

        }

        public virtual void Pickup(Inventory inventory)
        {
            inventory.AddToInventory(this);
        }

        public virtual Sprite GetIcon()
        {
            return _icon;
        }
    }
    
}
