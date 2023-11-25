using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Game;
using NOOD;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SellOption
{
    Prepare,
    All,
    One,
    Half
}
public enum BuyOption
{
    Prepare,
    One,
    Ten,
    OneHundred
}

public class ShopController : MonoBehaviorInstance<ShopController>
{
    [SerializeField] private MenuController _playerInventoryMenu, _shopMenu;
    [SerializeField] private OptionHolder _sellOption, _buyOption;
    [SerializeField] private Storable _money;

    public void Sell(SellOption sellOption)
    {
        MenuElement currentMenuElement = EventSystem.current.currentSelectedGameObject.GetComponent<MenuElement>();
        ItemSlot currentItemSlot = currentMenuElement.GetItemSlot();

        switch (sellOption)
        {
            case SellOption.Prepare:
                _sellOption.DisplayOptions();
                break;
            case SellOption.All:
                Debug.Log("Sell all");
                SellAll(currentItemSlot);
                break;
            case SellOption.One:
                Debug.Log("Sell one");
                SellOne(currentItemSlot);
                break;
            case SellOption.Half:
                Debug.Log("Sell half");
                SellHalf(currentItemSlot);
                break;
        }
        _playerInventoryMenu.UpdateUI();
    }
    public void Buy(BuyOption buyOption)
    {
        MenuElement currentShopItem = EventSystem.current.currentSelectedGameObject.GetComponent<MenuElement>();
        switch (buyOption)
        {
            case BuyOption.Prepare:
                _buyOption.DisplayOptions();
                break;
            case BuyOption.One:
                break;
            case BuyOption.Ten:
                break;
            case BuyOption.OneHundred:
                break;
        }
    }

    #region Sell
    private void SellAll(ItemSlot itemSlot)
    {
        if(itemSlot.count > 0 && itemSlot.storable != null)
        {
            int moneyCount = itemSlot.count * itemSlot.storable.Price;
            _playerInventoryMenu.ItemContainer.Remove(itemSlot.storable, itemSlot.count);
            _playerInventoryMenu.ItemContainer.Add(_money, moneyCount);
        }
    }
    private void SellHalf(ItemSlot itemSlot)
    {
        if(itemSlot.count > 1 && itemSlot.storable != null)
        {
            int moneyCount = itemSlot.count/2 * itemSlot.storable.Price;
            _playerInventoryMenu.ItemContainer.Remove(itemSlot.storable, itemSlot.count);
            _playerInventoryMenu.ItemContainer.Add(_money, moneyCount);
        }
        else
        {
            SellAll(itemSlot);
        }
    }
    private void SellOne(ItemSlot itemSlot)
    {
        if(itemSlot.count > 0 && itemSlot.storable != null)
        {
            int moneyCount = itemSlot.storable.Price;
            _playerInventoryMenu.ItemContainer.Remove(itemSlot.storable, itemSlot.count);
            _playerInventoryMenu.ItemContainer.Add(_money, moneyCount);
        }
    }
    #endregion

    #region Buy
    private void Buy1(ItemSO item)
    {

    }
    private void Buy10(ItemSO item)
    {

    }
    private void Buy100(ItemSO item)
    {

    }
    #endregion
}
