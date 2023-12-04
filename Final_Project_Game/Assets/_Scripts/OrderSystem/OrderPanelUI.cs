using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;


public class OrderPanelUI : MonoBehaviour
{
    #region SerializeField
    [SerializeField] private OrderElementUI _orderElementPref;
    [SerializeField] private Transform _orderElementHolder;
    [SerializeField] private MMF_Player _showFeedback, _hideFeedback;
    #endregion

    #region private Lists
    private List<Order> _orderDatas = new List<Order>();
    private List<OrderElementUI> _orderElements = new List<OrderElementUI>();
    #endregion

    #region Unity Functions
    void Start()
    {
        _orderElementPref.gameObject.SetActive(false);
        OrderManager.Instance.onPlayerOpenOrderPanel += UpdateUI;
        OrderManager.Instance.onPlayerOpenOrderPanel += Show;
    }
    #endregion

    #region Update
    private void DisableAllElement()
    {
        foreach(var element in _orderElements)
        {
            element.gameObject.SetActive(false);
        }
    }
    public void UpdateUI()
    {
        DisableAllElement();
        _orderDatas = OrderManager.Instance.GetOrderList();
        for(int i = 0; i < _orderDatas.Count; i++)
        {
            OrderElementUI orderElementUI;
            if(i == _orderElements.Count)
            {
                orderElementUI = CreateNewOrderElementUI();
            }
            else
                orderElementUI = _orderElements[i];
            orderElementUI.gameObject.SetActive(true);
            orderElementUI.Show();
            orderElementUI.UpdateUI(_orderDatas[i]);
            _orderElements.Add(orderElementUI);
        }
    }
    private OrderElementUI CreateNewOrderElementUI()
    {
        return Instantiate(_orderElementPref, _orderElementHolder);
    }
    #endregion

    #region Show Hide
    public void Show()
    {
        // Play feedback
        _showFeedback.PlayFeedbacks();
    }
    public void Hide()
    {
        // Play feedback
        _hideFeedback.PlayFeedbacks();
        // Deactivate all element when off
    }
    #endregion
}
