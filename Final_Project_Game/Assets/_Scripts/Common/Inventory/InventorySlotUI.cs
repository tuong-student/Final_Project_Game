using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] private Sprite _noneImage;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TMPro.TextMeshProUGUI _quantity;

        void Awake()
        {
            _itemIcon.sprite = _noneImage;
        }

        public void SetItemStack(ItemStack stack)
        {
            _itemIcon.sprite = stack.item.GetIcon();
            _quantity.text = stack.quantity.ToString();
        }
    }

}
