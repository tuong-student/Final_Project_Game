using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

public class Truck : Interactable
{
    #region SerializedField
    [SerializeField] private List<GameObject> _containers = new List<GameObject>();
    [SerializeField] private float _deliveryTime = 10f;
    #endregion

    #region Private
    private int _maxCapacity = 4;
    private int _currentCapacity = 0;
    private int _moneyOnTrip = 0;
    #endregion

    #region Unity functions
    void Start()
    {
        OrderManager.Instance.onPlayerCompleteOrder += PlayerCompleteOrder;
        DeactivateAllContainer();
    }
    #endregion

    #region Setup functions
    private void Go()
    {
        NoodyCustomCode.StartDelayFunction(Return, _deliveryTime);
        Debug.Log("Go");
    }
    private void Return()
    {
        _currentCapacity = 0;
        _moneyOnTrip = 0;
        DeactivateAllContainer();
        Debug.Log("Return");
    }
    private void DeactivateAllContainer()
    {
        foreach(var contain in _containers)
        {
            contain.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Event functions
    public void OpenOrderPanel()
    {
        OrderManager.Instance.onPlayerOpenOrderPanel?.Invoke();
    }
    public void PlayerCompleteOrder(int moneyOfOrder)
    {
        _moneyOnTrip += moneyOfOrder;
        _currentCapacity++;
        _containers[_currentCapacity - 1].SetActive(true);

        if(_currentCapacity == _maxCapacity)
        {
            Go();
        }
    }
    #endregion

    #region Interact
    public override void Interact(Character character)
    {
        OpenOrderPanel();
    }
    #endregion

    #region Add order to truck
    public void AddOrder()
    {
        _currentCapacity += 1;
        UpdateTruck();
    }
    private void UpdateTruck()
    {
       for(int i = 0; i < _currentCapacity; i++)
       {
            _containers[i].SetActive(true);
       } 
    }
    #endregion
}
