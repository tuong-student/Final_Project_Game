using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using NOOD.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderElementUI : MonoBehaviour
{
    #region SerializeField
    [SerializeField] private ImageOrder _imageOrderPref;
    [SerializeField] private Transform _orderItemHolder;
    [SerializeField] private TextMeshProUGUI _money;
    [SerializeField] private MMF_Player _completeFB;
    #endregion

    #region private
    private List<ImageOrder> _imageOrders = new List<ImageOrder>();
    private Order _orderData;
    #endregion

    #region Unity functions
    void Start()
    {
        _imageOrderPref.gameObject.SetActive(false);
        _completeFB.Events.OnComplete.AddListener(Hide);
    }
    #endregion

    #region Update and support functions
    private void DisableAllImage()
    {
        foreach(ImageOrder imageOrder in _imageOrders)
        {
            imageOrder.gameObject.SetActive(false);
        }
    }
    public void UpdateUI(Order order)
    {
        DisableAllImage();
        ImageOrder imageOrder;
        _orderData = order;
        for(int i = 0; i < order._orderItems.Count; i++)
        {
            OrderItem orderItem = order._orderItems[i];
            if(i ==_imageOrders.Count)
            {
                imageOrder = CreateNewImageOrder();
                _imageOrders.Add(imageOrder);
            }
            else
            {
                imageOrder = _imageOrders[i];
            }

            imageOrder.gameObject.SetActive(true);
            imageOrder.SetData(orderItem);
            if(orderItem.quantity <= 0)
            {
                imageOrder.ItemIcon.color = new Color(1, 1, 1, 1);
            }
            else
            {
                imageOrder.ItemIcon.color = new Color(1, 1, 1, 0.5f);
            }
        }
        _money.text = order._money.ToString("0");
    }
    private ImageOrder CreateNewImageOrder()
    {
        return Instantiate(_imageOrderPref, _orderItemHolder);
    }
    #endregion

    #region Check complete
    public void CheckComplete()
    {
        if(_orderData.IsComplete)
        {
            _completeFB.PlayFeedbacks();
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.OrderComplete);
        }
    }
    #endregion

    #region Show Hide
    public void Show()
    {
        // Play feedback
        this.gameObject.GetComponent<CanvasGroup>().alpha = 1;
    }
    public void Hide()
    {
        // Play feedback and disable
        Debug.Log("Hide");
        this.gameObject.SetActive(false);
    }
    #endregion
}
