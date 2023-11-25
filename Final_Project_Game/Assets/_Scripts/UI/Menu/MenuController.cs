using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using ImpossibleOdds;
using NOOD;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<MenuElement> _menuElements = new List<MenuElement>();
    [SerializeField] private ItemContainer _itemContainer;
    public ItemContainer ItemContainer => _itemContainer;
    [SerializeField] private Button _previousPage, _nextPage; 
    private CanvasGroup _canvasGroup;
    private int _page;
    private int _lastIndex;
    private MenuElement _lastMenuElementSelected;

    void Awake()
    {
        _menuElements = GetComponentsInChildren<MenuElement>().ToList();
        if(!TryGetComponent<CanvasGroup>(out _canvasGroup))
        {
            _canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        }
        _page = 0;
        DisplayItem();

        if(_previousPage != null)
            _previousPage.onClick.AddListener(PreviousPage);
        if(_nextPage != null)
            _nextPage.onClick.AddListener(NextPage);
    }

    void OnEnable()
    {
        SubscribeEvents();
        SelectElement(_menuElements[0]);
    }

    void OnDisable()
    {
        UnSubscribeEvents();
    }

    public void UpdateUI()
    {
        DisplayItem();
    }

    private void DisplayItem()
    {
        if(_itemContainer.slots.Count <= 24 * _page)
            _page = 0;

        foreach(var element in _menuElements)
        {
            element.SetItemSlot(null);
        }

        for(int i = _page * 24, j = 0; i < 24; i++)
        {
            if(i >= _itemContainer.slots.Count) break;
            _menuElements[j].SetItemSlot(_itemContainer.slots[i]);
            j++;
            _lastIndex = i;
        }
    }
    private void NextPage()
    {
        _page++;
        DisplayItem();
    }
    private void PreviousPage()
    {
        _page--;
        DisplayItem();
    }

    private void SubscribeEvents() 
    {
        GameInput.onPlayerAccept += PlayerAccept;
    }
    private void UnSubscribeEvents()
    {
        NoodyCustomCode.UnSubscribeFromStatic(typeof(GameInput), this);
    }

    private void SelectElement(MenuElement menuElement)
    {
        EventSystem.current.SetSelectedGameObject(menuElement.gameObject);
    }

    private void PlayerAccept()
    {
        if(EventSystem.current.currentSelectedGameObject.TryGetComponent<MenuElement>(out MenuElement currentMenuElement))
        {
            if(currentMenuElement != null && _menuElements.Contains(currentMenuElement))
            {
                // Check if current menu element belong to this menu controller
                _lastMenuElementSelected = currentMenuElement;
                currentMenuElement._optionHolder.DisplayOptions();
            }
        }
        OptionLogic.OnPlayerChooseClose += OnPlayerChooseClose;
        _canvasGroup.interactable = false;
        UnSubscribeEvents();
    }

    private void OnPlayerChooseClose()
    {
        if(_canvasGroup != null)
            _canvasGroup.interactable = true;
        if(_lastMenuElementSelected != null)
            EventSystem.current.SetSelectedGameObject(_lastMenuElementSelected.gameObject);
        NoodyCustomCode.UnSubscribeFromStatic(typeof(OptionLogic), this);
        SubscribeEvents();
    }
}