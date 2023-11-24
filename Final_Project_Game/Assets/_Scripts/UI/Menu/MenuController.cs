using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<MenuElement> _menuElements = new List<MenuElement>();
    private CanvasGroup _canvasGroup;
    private MenuElement _currentSelectedElement;
    private int _index;
    private bool _isBlock;

    void Awake()
    {
        _menuElements = GetComponentsInChildren<MenuElement>().ToList();
        if(!TryGetComponent<CanvasGroup>(out _canvasGroup))
        {
            _canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        }
    }

    void OnEnable()
    {
        SubscribeEvents();
        SelectElement(_menuElements[0]);
    }

    private void SubscribeEvents()
    {
        GameInput.onPlayerPressMoveVector2 += PlayerInputHandler;
        GameInput.onPlayerAccept += PlayerAccept;
    }
    private void UnSubscribeEvents()
    {
        GameInput.onPlayerPressMoveVector2 -= PlayerInputHandler;
        GameInput.onPlayerAccept -= PlayerAccept;
    }

    private void PlayerInputHandler(Vector2 playerInput)
    {
        // if(_isBlock) return;
        // if(playerInput.x > 0)
        // {
        //     _index += 1;
        // }
        // else if(playerInput.x < 0)
        // {
        //     _index -= 1;
        // }
        // else if(playerInput.y > 0)
        // {
        //     _index += 6;
        // }
        // else if(playerInput.y < 0)
        // {
        //     _index -= 6;
        // }
        // _index = Mathf.Clamp(_index, 0, 23);
        // SelectElement(_menuElements[_index]);
    }

    private void SelectElement(MenuElement menuElement)
    {
        // menuElement.Select();
        // _currentSelectedElement = menuElement;
        EventSystem.current.SetSelectedGameObject(menuElement.gameObject);
    }

    private void PlayerAccept()
    {
        // _currentSelectedElement._optionLogic.DisplayOption();
        // _isBlock = true;

        EventSystem.current.currentSelectedGameObject.GetComponent<OptionLogic>().DisplayOption();
        _canvasGroup.interactable = false;
        UnSubscribeEvents();
    }
}