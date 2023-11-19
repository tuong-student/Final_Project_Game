using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOOD.ModifiableStats;
using NOOD;

namespace Game
{
    public class PlayerManager : MonoBehaviorInstance<PlayerManager>
    {

        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private PlayerOnCollision _playerOnCollision;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private PlayerGun _playerGun;

        private ModifiableStats<float> _health = new ModifiableStats<float>();
        private ModifiableStats<float> _strength = new ModifiableStats<float>();
        private ModifiableStats<float> _speed = new ModifiableStats<float>();

        private List<Item> _items = new List<Item>();

        private PreviewHandler previewHandler;
        public ItemContainer inventoryContainer;


        void Awake()
        {
            GameInput.Init();
            GameInput.onPlayerPressInteract += Pickup;
            GameInput.onPlayerRequestItem += EquipItem;
            previewHandler = GameObject.Find("Grid").GetComponent<PreviewHandler>();
        }
        void Start()
        {
            _health.SetInitValue(PlayerConfig._maxHealth);
            _strength.SetInitValue(PlayerConfig._strength);
            _speed.SetInitValue(PlayerConfig._speed);
            UIManager.Instance.onPlayerDragOutItem += RemoveInventoryStack;
        }

        private void SelectObj()
        {
//            previewHandler.UpdateTile(tilemap, tileBase);
        }
        void Update()
        {
            // if(Input.GetKeyDown(KeyCode.Space))
            // {
            //     Debug.Log(TileManager.Instance.IsInteractable(this.transform.position.ToVector3Int()));
            //     TileManager.Instance.InteractableHere(this.transform.position.ToVector3Int());
            //     TileManager.Instance.AllPos();
            // }
        }

        public void ChangeGun(GunSO data)
        {
            _playerGun.ChangeGunData(data);
        }

        public void SetSpeed(float speed)
        {
            _playerMovement.SetSpeed(speed);
        }

        public void MinusHealth(float amount)
        {
            _health.AddModifier(ModifyType.Subtract, amount);
            FeedbackManager.Instance.PlayPlayerHurtFeedback();
            if(_health.Value <= 0)
            {
                Debug.Log("Death");
            }
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
