using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeawispHunter.RolePlay.Attributes;
using Game.Interface;
using NOOD;
using Sirenix.OdinInspector;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace Game
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private PlayerOnCollision _playerOnCollision;
        [SerializeField] private Inventory _inventory;
        // [SerializeField] private Transform _itemHolderTransform;

        private ModifiableValue<float> _health = new ModifiableValue<float>();
        private ModifiableValue<float> _strength = new ModifiableValue<float>();
        private ModifiableValue<float> _speed = new ModifiableValue<float>();

        private List<Item> _items = new List<Item>();

        private PreviewHandler previewHandler = new PreviewHandler();

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
            UIManager.Instance.onPlayerDragOutItem += RemoveInventoryStack;
        }

        private void SelectObj()
        {
//            previewHandler.UpdateTile(tilemap, tileBase);
        }
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(TileManager.Instance.IsInteractable(this.transform.position.ToVector3Int()));
                TileManager.Instance.InteractableHere(this.transform.position.ToVector3Int());
                TileManager.Instance.AllPos();
            }
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
            //tempItem.Pickup(_inventory);
            UIManager.Instance.UpdateInventoryUI(_inventory.GetInventoryStack());
        }
        public void AddToInventory(Item item)
        {
            _inventory.AddToInventory(item);
            UIManager.Instance.UpdateInventoryUI(_inventory.GetInventoryStack());
        }
        public void RemoveInventoryStack(InventoryStack stack)
        {
            _inventory.RemoveFromInventory(stack);
            UIManager.Instance.UpdateInventoryUI(_inventory.GetInventoryStack());
        }

        private void EquipItem(int inventoryIndex)
        {
            // Item item = _inventory.GetItemBaseOnIndex(inventoryIndex);
            // Item cloneItem = Instantiate(item, _itemHolderTransform); // Create Item GameObject
            // cloneItem.transform.position = _itemHolderTransform.position;
            // _playerAnimation.SetHold(true);
        }
    }
}
