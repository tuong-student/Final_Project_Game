using System.Linq;
using DG.Tweening;
using Game;
using NOOD;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [SerializeField] private Button _doneBtn;
    [SerializeField] private MenuController _playerInventoryMenu, _shopMenu;
    [SerializeField] private OptionHolder _sellOption, _buyOption;
    [SerializeField] private Storable _money;
    private CanvasGroup _canvasGroup;
    private MenuElement _playerItem, _shopItem;
    ItemSlot _currentItemSlot;

    #region Unity Functions
    void Awake()
    {
        if(this.TryGetComponent<CanvasGroup>(out _canvasGroup) == false)
        {
            _canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        }
        _doneBtn.onClick.AddListener(Close);
        _canvasGroup.alpha = 0;
    }
    #endregion

    #region Open Close
    public void Open()
    {
        this.gameObject.transform.DOScale(1, 1f);
        _canvasGroup.DOFade(1, 0.7f);
        UIManager.Instance.AddToUIList(this);
    }
    public void Close()
    {
        this.gameObject.transform.DOScale(0.3f, 1f);
        _canvasGroup.DOFade(0, 0.7f).OnComplete(() => this.gameObject.SetActive(false));
        UIManager.Instance.RemoveToUIList(this);
    }
    #endregion

    #region Sell
    public void Sell(SellOption sellOption)
    {

        switch (sellOption)
        {
            case SellOption.Prepare:
                _sellOption.DisplayOptions(CustomEventSystem.Instance.LastSelectedObject.transform.position);
                _playerItem = CustomEventSystem.Instance.LastSelectedObject.GetComponent<MenuElement>();
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
    public void Buy(BuyOption buyOption)
    {
        switch (buyOption)
        {
            case BuyOption.Prepare:
                _buyOption.DisplayOptions(CustomEventSystem.Instance.LastSelectedObject.transform.position);
                _shopItem = CustomEventSystem.Instance.LastSelectedObject.GetComponent<MenuElement>();
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
    private void Buy1(ItemSO item)
    {
        int cost = item.price;
        if(GetMoney() >= cost)
        {
            _playerInventoryMenu.ItemContainer.Add(item, 1);
            _playerInventoryMenu.ItemContainer.Remove(_money, cost);
        }
        else
        {
            WarningUI.Instance.ShowWarning(WarningType.NotEnoughMoney);
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
            WarningUI.Instance.ShowWarning(WarningType.NotEnoughMoney);
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
            WarningUI.Instance.ShowWarning(WarningType.NotEnoughMoney);
        }
    }
    private int GetMoney()
    {
        ItemSlot montySlot = _playerInventoryMenu.ItemContainer.slots.First(x => x.storable == _money);
        return montySlot.count;
    }
    #endregion
}
