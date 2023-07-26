using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOOD;

namespace Game
{
    public class UIManager : MonoBehaviorInstance<UIManager>
    {
        public Action<InventoryStack> onPlayerDragOutItem;

        [SerializeField] private InventoryUI _inventoryUI;

        protected override void Awake()
        {
            _inventoryUI = InventoryUI.Create(this.transform);
            base.Awake();
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }
        // Update is called once per frame
        void Update()
        {
            
        }

        public void UpdateInventoryUI(List<InventoryStack> inventoryStacks)
        {
            _inventoryUI.SetInventoryStacks(inventoryStacks);
        }
    }

}
