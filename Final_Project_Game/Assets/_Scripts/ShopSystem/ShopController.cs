using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private MenuElement _playerItem, _shopItem;
    ItemSlot _currentItemSlot;

    public void Sell(SellOption sellOption)
    {

        switch (sellOption)
        {
            case SellOption.Prepare:
                _sellOption.DisplayOptions();
                _playerItem = EventSystem.current.currentSelectedGameObject.GetComponent<MenuElement>();
                _currentItemSlot = _playerItem.GetItemSlot();
                break;
            case SellOption.All:
                Debug.Log("Sell all");
                SellAll(_currentItemSlot);
                break;
            case SellOption.One:
                Debug.Log("Sell one");
                SellOne(_currentItemSlot);
                break;
            case SellOption.Half:
                Debug.Log("Sell half");
                SellHalf(_currentItemSlot);
                break;
        }
        _playerInventoryMenu.UpdateUI();
    }
    public void Buy(BuyOption buyOption)
    {
        switch (buyOption)
        {
            case BuyOption.Prepare:
                _buyOption.DisplayOptions();
                _shopItem = EventSystem.current.currentSelectedGameObject.GetComponent<MenuElement>();
                _currentItemSlot = _shopItem.GetItemSlot();
                break;
            case BuyOption.One:
                Debug.Log("Buy 1");
                Buy1(_currentItemSlot.storable as ItemSO);
                break;
            case BuyOption.Ten:
                Debug.Log("Buy 10");
                Buy10(_currentItemSlot.storable as ItemSO);
                break;
            case BuyOption.OneHundred:
                Debug.Log("Buy 100");
                Buy100(_currentItemSlot.storable as ItemSO);
                break;
        }
        _playerInventoryMenu.UpdateUI();
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
            int moneyCount = (int)itemSlot.count/2 * itemSlot.storable.Price;
            _playerInventoryMenu.ItemContainer.Remove(itemSlot.storable, (int)itemSlot.count/2);
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
            _playerInventoryMenu.ItemContainer.Remove(itemSlot.storable, 1);
            _playerInventoryMenu.ItemContainer.Add(_money, moneyCount);
        }
    }
    #endregion

    #region Buy
    private void Buy1(ItemSO item)
    {
        int cost = item.price;
        if(GetMoney() >= cost)
        {
            _playerInventoryMenu.ItemContainer.Add(item, 1);
            _playerInventoryMenu.ItemContainer.Remove(_money, cost);
        }
    }
    private void Buy10(ItemSO item)
    {
        int cost = item.price * 10;
        if(GetMoney() >= cost)
        {
            _playerInventoryMenu.ItemContainer.Add(item, 10);
            _playerInventoryMenu.ItemContainer.Remove(_money, cost);
        }
        else
        {

        }
    }
    private void Buy100(ItemSO item)
    {
        int cost = item.price * 100;
        if(GetMoney() >= cost)
        {
            _playerInventoryMenu.ItemContainer.Add(item, 100);
            _playerInventoryMenu.ItemContainer.Remove(_money, cost);
        }
        else
        {

        }
    }
    private int GetMoney()
    {
        ItemSlot montySlot = _playerInventoryMenu.ItemContainer.slots.First(x => x.storable == _money);
        return montySlot.count;
    }
    #endregion
}
