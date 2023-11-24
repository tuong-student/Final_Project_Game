using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class InventorySlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler
    {
        [SerializeField] private Sprite _noneImage;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TMPro.TextMeshProUGUI _quantityText;
        [SerializeField] private ItemStack _itemStackPref;
        private Item _item;
        private int _quantity;
        private Image _tempImage;
        private InventoryStack _inventoryStack;

        void Awake()
        {
            _itemIcon.sprite = _noneImage;
        }

        public void SetInventoryStack(InventoryStack stack)
        {
            if(stack == null)
            {
                _itemIcon.sprite = _noneImage;
                _quantity = 0;
                _item = null;
            }
            else
            {
             //   _itemIcon.sprite = stack._item.GetIcon();
                _quantity = stack._quantity;
                _item = stack._item;
            }
            _quantityText.text = _quantity.ToString("0");
            _inventoryStack = stack;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(_itemIcon.sprite == _noneImage) return;

            Vector3 worldPos = NOOD.NoodyCustomCode.ScreenPointToWorldPoint(eventData.position);
            worldPos.z = 0;
            ItemStack itemStack = Instantiate<ItemStack>(_itemStackPref, worldPos, Quaternion.identity);
            //itemStack.SetItemAndQuantity(_item, _quantity);

            Destroy(_tempImage);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(_itemIcon.sprite == _noneImage) return;
            _tempImage.rectTransform.anchoredPosition += eventData.delta / _tempImage.canvas.scaleFactor;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(_itemIcon.sprite == _noneImage) return;
            _tempImage = Instantiate(_itemIcon, this.transform);
        }
    }

}
