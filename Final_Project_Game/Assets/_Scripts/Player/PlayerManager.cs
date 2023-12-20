using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOOD.ModifiableStats;
using NOOD;
using NOOD.Sound;
using System.Linq;

namespace Game
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerOnCollision))]
    public class PlayerManager : MonoBehaviorInstance<PlayerManager>
    {
        #region Action
        public Action OnPlayerInventoryChange;
        #endregion

        #region SerializeField
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private PlayerOnCollision _playerOnCollision;
        [SerializeField] private AbstractItem _playerGun;
        [SerializeField] private ItemContainer _inventoryContainer;
        #endregion

        #region Private parameter
        private ModifiableStats<float> _health = new ModifiableStats<float>();
        private ModifiableStats<float> _strength = new ModifiableStats<float>();
        private ModifiableStats<float> _speed = new ModifiableStats<float>();
        private List<ItemSO> _items = new List<ItemSO>();
        private PreviewHandler previewHandler;
        #endregion

        #region Public
        public ItemContainer InventoryContainer => _inventoryContainer;
        public int Money 
        {
            get
            {
                int money = 0;
                if(_inventoryContainer.slots.Any(x => x.storable == _coin))
                {
                    money = _inventoryContainer.slots.First(x => x.storable == _coin).count;
                }
                return money;
            }
        } 
        #endregion

        [SerializeField] private ItemSO _coin;

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
        public void MinusHealth(float amount)
        {
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.PlayerHurt);
            _health.AddModifier(ModifyType.Subtract, amount);
            FeedbackManager.Instance.PlayPlayerHurtFeedback();

            if(_health.Value <= 0)
            {
                Dead();
            }
        }
        private void Dead()
        {
            NoodyCustomCode.StartDelayFunction(_playerAnimation.PlayDeadAnimation, 0.2f);
            NoodyCustomCode.StartDelayFunction(() => 
            {
                _playerGun.gameObject.SetActive(false);
            }, 1.5f);
            NoodyCustomCode.StartDelayFunction(() =>
            {
                UIManager.Instance.ActiveDeadMenu();
            }, 3f);
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.PlayerDead);
        }
        #endregion

        #region Inventory
        public void AddToInventory(Storable item, int count = 1)
        {
            _inventoryContainer.Add(item, count);
            OnPlayerInventoryChange?.Invoke();
        }
        public void AddDefaultItems(Dictionary<Storable, int> itemsAndCount)
        {
            foreach(var itemsPair in itemsAndCount)
            {
                _inventoryContainer.Add(itemsPair.Key, itemsPair.Value);
            }
        }
        public void RemoveFromInventory(Storable item, int count)
        {
            _inventoryContainer.Remove(item, count);
            OnPlayerInventoryChange?.Invoke();
        }
        public bool TryRemoveInventory(Storable item, int count)
        {
            if(_inventoryContainer.ContainItem(item) == false)
            {
                return false;
            }
            else
            {
                if(_inventoryContainer.GetSlot(item).count < count)
                {
                    return false;
                }
            }
            RemoveFromInventory(item, count);
            return true;
        }
        public void ClearAllInventory()
        {
            for (int i = 0; i < _inventoryContainer.slots.Count; i++)
            {
                _inventoryContainer.slots[i].Clear();
            }
        }
        #endregion
    }
}
