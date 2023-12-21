using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using Game;
using MoreMountains.Feedbacks;
using AssetKits.ParticleImage;
using NOOD;
using NOOD.Sound;

public class ImageOrder : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDisplayInfo
{
    #region SerializeField
    [SerializeField] private MMF_Player _minusFB;
    [SerializeField] private MMF_Player _completeFB;
    #endregion

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
        GetComponentInParent<OrderElementUI>().CheckComplete();
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.OrderItemComplete);
    }
    #endregion

    #region UpdateUI
    private void CompleteUI()
    {
        // Play complete feedback
        _completeFB.PlayFeedbacks();
    }
    private bool MinusOne()
    {
        bool canGetItem = PlayerManager.Instance.TryRemoveInventory(_orderItemData.storable, 1);
        if(canGetItem)
        {
            _orderItemData.quantity -= 1;
            _minusFB.PlayFeedbacks();
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.Pop);
            UpdateUI();
        }
        else
        {
            WarningUI.Instance.ShowWarning(WarningType.DontHaveItem);
        }
        return canGetItem;
    }
    private void UpdateUI()
    {
        _itemIcon.sprite = _orderItemData.storable.IconImage;
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

    #region Interface functions
    public (string, Color) GetName()
    {
        if(_orderItemData == null)
        {
            return (null, Color.white);
        }
        else
        {
            return (_orderItemData.storable.name, NoodyCustomCode.HexToColor("#5A00FF"));
        }
    }
    #endregion
}

[Serializable]
public class OrderItem
{
    public Storable storable;
    public int quantity;
}