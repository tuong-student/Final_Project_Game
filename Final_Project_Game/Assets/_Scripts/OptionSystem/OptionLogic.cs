using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OptionLogic 
{
    public static Action OnPlayerChooseClose;

    public static void PerformOption(OptionDataSO optionDataSO)
    {
        switch (optionDataSO._actionToActive)
        {
            case ActionType.Shop:
                Debug.Log("Shop");
                UIManager.Instance.LoadUI(UI.Shop);
                break;
            case ActionType.Close:
                Debug.Log("Close");
                OptionUI.Instance.Close();
                OnPlayerChooseClose?.Invoke();
                break;
            case ActionType.Sell:
                Debug.Log("Sell");
                // Call Shop System to perform sell action
                ShopController.Instance.Sell(SellOption.Prepare);
                break;
            case ActionType.Buy:
                Debug.Log("Buy");
                // Call Shop system to perform buy action
                ShopController.Instance.Buy(BuyOption.Prepare);
                break;
            case ActionType.SellOne:
                ShopController.Instance.Sell(SellOption.One);
                break;
            case ActionType.SellHalf:
                ShopController.Instance.Sell(SellOption.Half);
                break;
            case ActionType.SellAll:
                ShopController.Instance.Sell(SellOption.All);
                break;
            case ActionType.Buy1:
                ShopController.Instance.Buy(BuyOption.One);
                break;
            case ActionType.Buy10:
                ShopController.Instance.Buy(BuyOption.Ten);
                break;
            case ActionType.Buy100:
                ShopController.Instance.Buy(BuyOption.OneHundred);
                break;
        }
    }
}
