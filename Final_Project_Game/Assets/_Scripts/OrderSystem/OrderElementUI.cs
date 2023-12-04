using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderElementUI : MonoBehaviour
{
    [SerializeField] private ImageOrder _imageOrderPref;
    [SerializeField] private Transform _orderItemHolder;
    [SerializeField] private TextMeshProUGUI _money;
    private List<ImageOrder> _imageOrders = new List<ImageOrder>();
    private Order _orderData;

    #region Unity functions
    void Start()
    {
        _imageOrderPref.gameObject.SetActive(false);
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
        }
        _money.text = order._money.ToString("0");
    }
    private ImageOrder CreateNewImageOrder()
    {
        return Instantiate(_imageOrderPref, _orderItemHolder);
    }
    #endregion

    #region Show Hide
    public void Show()
    {
        // Play feedback
    }
    #endregion
}
