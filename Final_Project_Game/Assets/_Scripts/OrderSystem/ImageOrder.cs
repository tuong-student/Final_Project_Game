using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using Game;

public class ImageOrder : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Private
    private OrderItem _orderItemData;
    private bool _isPress;
    private float _timer;
    private float _nextMinusTime;
    #endregion

    #region Public
    public Image _itemIcon;
    public TextMeshProUGUI _itemQuantity;
    public bool IsComplete => _orderItemData.quantity <= 0;
    #endregion

    #region Unity functions
    void OnEnable()
    {
        _nextMinusTime = Time.time;
        _timer = Time.time;
    }
    void Update()
    {
        _timer += Time.deltaTime;
        if(_isPress && _timer > _nextMinusTime && IsComplete == false)
        {
            _timer = Time.time;
            _nextMinusTime = Time.time + 0.2f;
            if(MinusOne())
            {
                // Success
                if(IsComplete == true)
                {
                    CompleteUI();
                    CompleteLogic();
                }
            }
            else
            {
                // Fail
            }
        }
    }
    #endregion

    #region Init data
    public void SetData(OrderItem orderItem)
    {
        _orderItemData = orderItem;
        UpdateUI();
    }
    #endregion

    #region Update logic
    private void CompleteLogic()
    {
        OrderManager.Instance.CheckComplete();
    }
    #endregion

    #region UpdateUI
    private void CompleteUI()
    {
        // Play complete feedback
    }
    private bool MinusOne()
    {
        bool value = PlayerManager.Instance.TryRemoveInventory(_orderItemData.storable, 1);
        if(value)
        {
            _orderItemData.quantity -= 1;
            UpdateUI();
        }
        return value;
    }
    private void UpdateUI()
    {
        _itemIcon.sprite = _orderItemData.storable.Icon;
        _itemQuantity.text = _orderItemData.quantity.ToString("0");
    }
    #endregion

    #region Pointer events
    public void OnPointerDown(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        _isPress = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
        _isPress = false;
    }
    #endregion
}

[Serializable]
public class OrderItem
{
    public Storable storable;
    public int quantity;
}