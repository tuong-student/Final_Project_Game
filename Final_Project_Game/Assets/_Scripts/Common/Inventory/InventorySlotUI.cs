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

        void Awake()
        {
            _itemIcon.sprite = _noneImage;
        }

        public void SetInventoryStack(InventoryStack stack)
        {
            _itemIcon.sprite = stack._item.GetIcon();
            _quantityText.text = stack._quantity.ToString();
            _item = stack._item;
            _quantity = stack._quantity;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Vector3 worldPos = NOOD.NoodyCustomCode.ScreenPointToWorldPoint(eventData.position);
            worldPos.z = 0;
            ItemStack itemStack = Instantiate<ItemStack>(_itemStackPref, worldPos, Quaternion.identity);
            itemStack.SetItemAndQuantity(_item, _quantity);

            Destroy(_tempImage);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _tempImage.rectTransform.anchoredPosition += eventData.delta / _tempImage.canvas.scaleFactor;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _tempImage = Instantiate(_itemIcon, this.transform);
        }
    }

}
