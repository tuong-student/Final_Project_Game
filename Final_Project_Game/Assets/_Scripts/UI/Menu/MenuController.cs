using System.Collections.Generic;
using System.Linq;
using Game;
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

    #region Unity Functions
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
    #endregion

    #region DisplayItem
    private void DisplayItem()
    {
        if(_itemContainer.slots.Count <= 24 * (_page + 1) || _page < 0)
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
    #endregion

    #region Events
    private void SubscribeEvents() 
    {
        GameInput.onPlayerAccept += PlayerAccept;
        GameInput.onPlayerPressMoveVector2 += PlayerMoveHandler;
    }
    private void UnSubscribeEvents()
    {
        // NoodyCustomCode.UnSubscribeFromStatic(typeof(GameInput), this);
        GameInput.onPlayerAccept -= PlayerAccept;
        GameInput.onPlayerPressMoveVector2 -= PlayerMoveHandler;
    }
    private void OnOptionMenuClose()
    {
        if(_canvasGroup != null)
            _canvasGroup.interactable = true;
        if(_lastMenuElementSelected != null)
            EventSystem.current.SetSelectedGameObject(_lastMenuElementSelected.gameObject);
        NoodyCustomCode.UnSubscribeFromStatic(typeof(OptionLogic), this);
        SubscribeEvents();
    }
    #endregion

    #region Select 
    public void SetLastSelectedObject(MenuElement element)
    {
        _lastMenuElementSelected = element;
    }
    private void SelectElement(MenuElement menuElement)
    {
        EventSystem.current.SetSelectedGameObject(menuElement.gameObject);
    }
    #endregion

    #region Player Input
    private void PlayerMoveHandler(Vector2 playerInput)
    {
        if(playerInput != Vector2.zero && _lastMenuElementSelected != null && EventSystem.current.currentSelectedGameObject == null)
        {
            SelectElement(_lastMenuElementSelected);
        }
    }
    private void PlayerAccept()
    {
        if (EventSystem.current.currentSelectedGameObject == null) return;
        if(EventSystem.current.currentSelectedGameObject.TryGetComponent<MenuElement>(out MenuElement currentMenuElement))
        {
            if(currentMenuElement != null && _menuElements.Contains(currentMenuElement))
            {
                // Check if current menu element belong to this menu controller
                _lastMenuElementSelected = currentMenuElement;
                currentMenuElement._optionHolder.DisplayOptions(CustomEventSystem.Instance.LastSelectedObject.transform.position);
            }
            OptionLogic.OnPlayerChooseClose += OnOptionMenuClose;
            if(_canvasGroup != null)
                _canvasGroup.interactable = false;
        }
        UnSubscribeEvents();
    }
    #endregion

    public void UpdateUI()
    {
        DisplayItem();
    }
}