using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private List<InventorySlotUI> _slots = new List<InventorySlotUI>();

        public static InventoryUI Create(Transform parent = null)
        {
            return Instantiate<InventoryUI>(Resources.Load<InventoryUI>("Prefabs/UI/InventoryHolder"), parent);
        }

        public void SetInventoryStacks(List<InventoryStack> stacks)
        {
            InventoryStack stack = null;
            for(int i = 0; i < _slots.Count; i++)
            {
                try
                {
                    stack = stacks[i];
                }
                catch
                {
                    stack = null;
                }
                _slots[i].SetInventoryStack(stack);
            }
        }
    }

}
