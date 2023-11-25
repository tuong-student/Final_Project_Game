using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;

namespace Game
{
    public class PlayerOnCollision : MonoBehaviour
    {
        private IInteractable _interactableObject;
        private IPickupable _pickupableObject;
        public event Action<object> onPlayerTriggerEnter;

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.GetComponent<IInteractable>() != null)
            {
                _interactableObject = other.GetComponent<IInteractable>();
            } 
            if(other.GetComponent<IPickupable>() != null)
            {
                _pickupableObject = other.GetComponent<IPickupable>();
            }
        }
        void OnTriggerExit2D(Collider2D other)
        {
            if(other.GetComponent<IInteractable>() != null)
            {
                _interactableObject = null;
            }
            if(other.GetComponent<IPickupable>() != null)
            {
                _pickupableObject = null;
            }
        }

        public ItemSO GetGroundItem()
        {
            return _interactableObject as ItemSO;
        }
        public IPickupable GetPickupableObject()
        {
            return _pickupableObject;
        }
    }
}
