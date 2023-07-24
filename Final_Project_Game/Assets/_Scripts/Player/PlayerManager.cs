using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeawispHunter.RolePlay.Attributes;
using Game.Interface;
using NOOD;

namespace Game
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private PlayerOnCollision _playerOnCollision;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private Transform _itemHolderTransform;

        private ModifiableValue<float> _health = new ModifiableValue<float>();
        private ModifiableValue<float> _strength = new ModifiableValue<float>();
        private ModifiableValue<float> _speed = new ModifiableValue<float>();

        private List<Item> _items = new List<Item>();

        void Awake()
        {
            GameInput.Init();
            GameInput.onPlayerPressInteract += Pickup;
            GameInput.onPlayerRequestItem += EquipItem;
        }
        void Start()
        {
            _health.initial.value = PlayerConfig._maxHealth;
            _strength.initial.value = PlayerConfig._strength;
            _speed.initial.value = PlayerConfig._speed;
        }
        void Update()
        {
        }

        public void SetSpeed(float speed)
        {
            _playerMovement.SetSpeed(speed);
        }

        public void MinusHealth(float amount)
        {
            Debug.Log("MinusHealth");
        }

        public void Pickup()
        {
            Item tempItem = _playerOnCollision.GetPickupableObject() as Item;
            if(tempItem == null) return;
            tempItem.Pickup(_inventory);
            UIManager.Instance.UpdateInventoryUI(_inventory.GetItemStacks());
        }
        public void AddToInventory(Item item)
        {
            _inventory.AddToInventory(item);
            UIManager.Instance.UpdateInventoryUI(_inventory.GetItemStacks());
        }

        private void EquipItem(int inventoryIndex)
        {
            Item item = _inventory.GetItemBaseOnIndex(inventoryIndex);
            Item cloneItem = Instantiate(item, _itemHolderTransform); // Create Item GameObject
            cloneItem.transform.position = _itemHolderTransform.position;
            _playerAnimation.SetHold(true);
        }
    }
}
