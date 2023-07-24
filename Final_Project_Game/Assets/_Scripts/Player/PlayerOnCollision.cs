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

        void OnTriggerEnter(Collider other)
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
        void OnTriggerExit(Collider other)
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

        public Item GetGroundItem()
        {
            return _interactableObject as Item;
        }
        public IPickupable GetPickupableObject()
        {
            return _pickupableObject;
        }
    }
}
