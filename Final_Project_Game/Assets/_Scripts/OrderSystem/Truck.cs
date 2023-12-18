using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using NOOD;
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
    private int _currentCapacity = 0;
    private int _moneyOnTrip = 0;
    #endregion

    #region Unity functions
    void Start()
    {
        OrderManager.Instance.onPlayerCompleteOrder += PlayerCompleteOrder;
        OrderManager.Instance.onPlayerPressGO += Go;
        DeactivateAllContainer();
    }
    #endregion

    #region Go and Return
    private void Go()
    {
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
        while(_moneyOnTrip > 0)
        {
            if(_moneyOnTrip / 10 > 0)
            {
                _moneyOnTrip /= 10;
                ItemSpawnManager.instance.SpawnItem(NoodyCustomCode.GetRandomPointInsideCollider2D(_truckBox), this.transform, _coinItem, 10);
            }
            else
            {
                ItemSpawnManager.instance.SpawnItem(NoodyCustomCode.GetRandomPointInsideCollider2D(_truckBox), this.transform, _coinItem, _moneyOnTrip);
                _moneyOnTrip = 0;
            }
        }
        _currentCapacity = 0;
        DeactivateAllContainer();
        this.transform.DOMoveX(_inXPosition, 3).SetEase(Ease.OutCirc);
        _positionShakeFB.PlayFeedbacks();
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
        _moneyOnTrip += moneyOfOrder;
        AddOrder();

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
        Debug.Log("Interact");
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
        _containerFBs[_currentCapacity - 1].PlayFeedbacks();
    }
    #endregion
}
