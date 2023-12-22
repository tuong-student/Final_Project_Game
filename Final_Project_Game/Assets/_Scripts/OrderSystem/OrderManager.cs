using System;
using System.Collections.Generic;
using System.Linq;
using NOOD;
using UnityEngine;

#region Order class
[Serializable]
public class Order
{
    public List<OrderItem> _orderItems = new List<OrderItem>();
    public int _money;
    public bool IsComplete 
    {
        get
        {
            if (_orderItems.Any(x => x.quantity > 0))
                return false;
            else
                return true;
        }
    }
}
#endregion

public class OrderManager : MonoBehaviorInstance<OrderManager>
{
    #region Events
    public Action onPlayerOpenOrderPanel;
    public Action onPlayerPressGO;
    public Action<int> onPlayerCompleteOrder;
    #endregion

    [SerializeField] private List<Storable> _storableSOs = new List<Storable>();
    [SerializeField] private List<Order> _orderList = new List<Order>();
    private int completeOrderNumber = 0;

    #region Unity functions
    void Start()
    {
        NoodyCustomCode.StartNewCoroutineLoop(() => {
            if(_orderList.Count < 5)
            {
                for(int i = 0; i < 4; i++)
                {
                    CreateOrder();
                }
            }
        }, 10f);
    }
    #endregion

    #region Get Set
    public List<Order> GetOrderList()
    {
        return _orderList;
    }
    #endregion

    #region Update logic
    public void CheckComplete()
    {
        List<Order> completeOrders = _orderList.FindAll(x => x.IsComplete == true);
        foreach(Order order in completeOrders)
        {
            if(completeOrderNumber < 4)
            {
                completeOrderNumber++;
                onPlayerCompleteOrder?.Invoke(order._money);
                _orderList.Remove(order);
            }
        }
    }
    #endregion

    #region Add Remove
    public Order CreateOrder()
    {
        Order newOrder = new Order();
        int quantity = 4;
        for(int i = 0; i < UnityEngine.Random.Range(1, 4); i++)
        {
            OrderItem orderItem = new OrderItem
            {
                storable = GetRandomStorable(),
                quantity = UnityEngine.Random.Range(1, 40)
            };

            newOrder._orderItems.Add(orderItem);
            quantity += orderItem.quantity;
        }
        newOrder._money = quantity * UnityEngine.Random.Range(1, 10);
        _orderList.Add(newOrder);
        return newOrder;
    }
    #endregion

    #region Support functions
    private Storable GetRandomStorable()
    {
        int r = UnityEngine.Random.Range(0, _storableSOs.Count);
        return _storableSOs[r];
    }
    #endregion
}
