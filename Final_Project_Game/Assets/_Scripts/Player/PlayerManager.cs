using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOOD.ModifiableStats;
using NOOD;

namespace Game
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerOnCollision))]
    public class PlayerManager : MonoBehaviorInstance<PlayerManager>
    {
        #region SerializeField
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private PlayerOnCollision _playerOnCollision;
        [SerializeField] private PlayerItem _playerGun;
        #endregion

        #region Private parameter
        private ModifiableStats<float> _health = new ModifiableStats<float>();
        private ModifiableStats<float> _strength = new ModifiableStats<float>();
        private ModifiableStats<float> _speed = new ModifiableStats<float>();

        private PreviewHandler previewHandler;
        public ItemContainer inventoryContainer;
        public ItemContainer InventoryContainer => inventoryContainer;
        #endregion

        #region Unity Events
        void Awake()
        {
            previewHandler = GameObject.Find("Grid").GetComponent<PreviewHandler>();
        }
        void Start()
        {
            _health.SetInitValue(PlayerConfig._maxHealth);
            _strength.SetInitValue(PlayerConfig._strength);
            _speed.SetInitValue(PlayerConfig._speed);
        }
        void OnEnable()
        {
            GameInput.Init();
            GameInput.onPlayerPressInteract += Pickup;
        }
        void OnDestroy()
        {
            NoodyCustomCode.UnSubscribeFromStatic(typeof(GameInput), this);
            GameInput.Dispose();
        }
        #endregion

        #region Get Set
        public void SetSpeed(float speed)
        {
            _playerMovement.SetSpeed(speed);
        }
        public float GetHealth()
        {
            return _health.Value;
        }
        public void MinusHealth(float amount)
        {
            _health.AddModifier(ModifyType.Subtract, amount);
            FeedbackManager.Instance.PlayPlayerHurtFeedback();
            if(_health.Value <= 0)
            {
                NoodyCustomCode.StartDelayFunction(_playerAnimation.PlayDeadAnimation, 0.2f);
                NoodyCustomCode.StartDelayFunction(() => this.gameObject.SetActive(false), 1.5f);
            }
        }
        #endregion

        #region SupportFunctions
        public void ChangeItem(IHoldableItem data)
        {
            _playerGun.ChangeItemData(data);
        }
        public void Pickup()
        {
            ItemSO tempItem = _playerOnCollision.GetPickupableObject() as ItemSO;
            if(tempItem == null) return;
            //tempItem.Pickup(_inventory);
        }
        #endregion

        #region Inventory
        public void AddToInventory(Storable item, int count = 1)
        {
            inventoryContainer.Add(item, count);
        }
        public void RemoveFromInventory(Storable item, int count)
        {
            inventoryContainer.Remove(item, count);
        }
        public bool TryRemoveInventory(Storable item, int count)
        {
            if(inventoryContainer.ContainItem(item) == false)
            {
                return false;
            }
            else
            {
                if(inventoryContainer.GetSlot(item).count < count)
                {
                    return false;
                }
            }
            RemoveFromInventory(item, count);
            return true;
        }
        public void ClearAllInventory()
        {
            for (int i = 0; i < inventoryContainer.slots.Count; i++)
            {
                inventoryContainer.slots[i].Clear();
            }
        }
        #endregion
    }
}
