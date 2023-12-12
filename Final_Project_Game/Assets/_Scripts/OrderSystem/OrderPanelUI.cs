using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;
using Game;


public class OrderPanelUI : MonoBehaviour
{
    #region SerializeField
    [SerializeField] private OrderElementUI _orderElementPref;
    [SerializeField] private Transform _orderElementHolder;
    [SerializeField] private MMF_Player _showFeedback, _hideFeedback;
    [SerializeField] private CustomButton _goBtn, _closeBtn;
    #endregion

    #region private Lists
    private List<Order> _orderDatas = new List<Order>();
    private List<OrderElementUI> _orderElements = new List<OrderElementUI>();
    private bool _isShowed = false;
    #endregion

    #region Unity Functions
    void Start()
    {
        OrderManager.Instance.onPlayerOpenOrderPanel += UpdateUI;
        OrderManager.Instance.onPlayerOpenOrderPanel += Show;
        _orderElementPref.gameObject.SetActive(false);
        _goBtn.SetAction(GoAction);
        _closeBtn.SetAction(Hide);
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

    #region GoAction
    private void GoAction()
    {
        OrderManager.Instance.onPlayerPressGO?.Invoke();
        Hide();
    }
    #endregion

    #region Show Hide
    public void Show()
    {
        // Play feedback
        if (_isShowed == true) return;
        _showFeedback.PlayFeedbacks();
        UIManager.Instance.AddToUIList(this);
    }
    public void Hide()
    {
        // Play feedback
        _hideFeedback.PlayFeedbacks();
        UIManager.Instance.RemoveToUIList(this);
        // Deactivate all element when off
    }
    #endregion
}
