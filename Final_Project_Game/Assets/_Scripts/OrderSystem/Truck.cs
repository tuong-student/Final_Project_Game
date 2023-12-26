using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using NOOD;
using NOOD.Data;
using NOOD.Sound;
using UnityEngine;

public class Truck : Interactable
{
    #region SerializedField
    [SerializeField] private List<MMF_Player> _containerFBs = new List<MMF_Player>();
    [SerializeField] private float _deliveryTime = 10f;
    [SerializeField] private float _inXPosition = -13, _outXPosition = -22;
    [SerializeField] private BoxCollider2D _truckBox;
    [SerializeField] private ItemSO _coinItem;
    [SerializeField] private MMF_Player _positionShakeFB;
    #endregion

    #region Private
    private int _maxCapacity = 4;
    private int _CurrentCapacity 
    {
        get 
        {
            return DataManager<TruckCapacity>.Data.currentCapacity;
        }
        set 
        {
            DataManager<TruckCapacity>.Data.currentCapacity = value;
            DataManager<TruckCapacity>.QuickSave();
        }
    }
    private int _MoneyOnTrip
    {
        get 
        {
            return DataManager<TruckCapacity>.Data.moneyOnTrip;
        }
        set
        {
            DataManager<TruckCapacity>.Data.moneyOnTrip = value;
            DataManager<TruckCapacity>.QuickSave();
        }
    }
    #endregion

    #region Unity functions
    void Start()
    {
        OrderManager.Instance.onPlayerCompleteOrder += PlayerCompleteOrder;
        OrderManager.Instance.onPlayerPressGO += Go;
        if (GameManager.instance.gameStatus.isNewGame)
            DataManager<TruckCapacity>.QuickClear();

    }
    void OnEnable()
    {
        if (_CurrentCapacity > 0)
        {
            for(int i = 0; i < _CurrentCapacity; i++)
            {
                MMF_Player currentFB = _containerFBs[i];
                if (currentFB != null)
                    currentFB.PlayFeedbacks();             
            }
        }
        else
            DeactivateAllContainer();
    }
    void OnDisable()
    {
        NoodyCustomCode.UnSubscribeAllEvent(OrderManager.Instance, this);
    }
    #endregion

    #region Go and Return
    private void Go()
    {
        if(_positionShakeFB != null)
            _positionShakeFB.PlayFeedbacks();
        NoodyCustomCode.StartDelayFunction(() =>
        {
            this.transform.DOMoveX(_outXPosition, 3).SetEase(Ease.InCirc);
        }, 3f);
        NoodyCustomCode.StartDelayFunction(Return, _deliveryTime);
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.Truck);
    }
    private void Return()
    {
        // Create money
        while(_MoneyOnTrip > 0)
        {
            if(_MoneyOnTrip / 10 > 0)
            {
                _MoneyOnTrip /= 10;
                ItemSpawnManager.instance.SpawnItem(NoodyCustomCode.GetRandomPointInsideCollider2D(_truckBox), this.transform, _coinItem, 10);
            }
            else
            {
                ItemSpawnManager.instance.SpawnItem(NoodyCustomCode.GetRandomPointInsideCollider2D(_truckBox), this.transform, _coinItem, _MoneyOnTrip);
                _MoneyOnTrip = 0;
            }
        }

        _CurrentCapacity = 0;

        DeactivateAllContainer();
        this.transform.DOMoveX(_inXPosition, 3).SetEase(Ease.OutCirc);
        _positionShakeFB.PlayFeedbacks();
        OrderManager.Instance.ResetOrderNumber();
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.Truck);
    }
    private void DeactivateAllContainer()
    {
        foreach(var contain in _containerFBs)
        {
            contain.transform.localScale = Vector3.zero;
        }
    }
    #endregion

    #region Event functions
    public void OpenOrderPanel()
    {
        OrderManager.Instance.onPlayerOpenOrderPanel?.Invoke();
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.InteractClick);
    }
    public void PlayerCompleteOrder(int moneyOfOrder)
    {
        _MoneyOnTrip += moneyOfOrder;
        AddOrder();

        if(_CurrentCapacity == _maxCapacity)
        {
            Go();
            OrderManager.Instance.onTruckGo?.Invoke();
        }
    }
    #endregion

    #region Interact
    public override void Interact(Character character)
    {
        OpenOrderPanel();
        Debug.Log("Interact");
    }
    #endregion

    #region Add order to truck
    public void AddOrder()
    {
        _CurrentCapacity += 1;
        UpdateTruck();
    }
    private void UpdateTruck()
    {
        MMF_Player currentFB = _containerFBs[_CurrentCapacity - 1];
        if (currentFB != null)
            currentFB.PlayFeedbacks();
    }
    #endregion
}

public class TruckCapacity
{
    public int currentCapacity;
    public int moneyOnTrip;
}
