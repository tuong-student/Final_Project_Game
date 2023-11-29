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

        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private PlayerOnCollision _playerOnCollision;
        [SerializeField] private PlayerGun _playerGun;

        private ModifiableStats<float> _health = new ModifiableStats<float>();
        private ModifiableStats<float> _strength = new ModifiableStats<float>();
        private ModifiableStats<float> _speed = new ModifiableStats<float>();

        private List<ItemSO> _items = new List<ItemSO>();

        private PreviewHandler previewHandler;
        public ItemContainer inventoryContainer;
        public ItemContainer InventoryContainer => inventoryContainer;

        #region Unity Events
        void Awake()
        {
            // previewHandler = GameObject.Find("Grid").GetComponent<PreviewHandler>();
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

        private void SelectObj()
        {
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
            ItemSO tempItem = _playerOnCollision.GetPickupableObject() as ItemSO;
            if(tempItem == null) return;
            //tempItem.Pickup(_inventory);
        }
        public void AddToInventory(Storable item, int count = 1)
        {
            inventoryContainer.Add(item, count);
        }
        public void RemoveFromInventory(Storable item, int count)
        {
            inventoryContainer.Remove(item, count);
        }
    }
}
