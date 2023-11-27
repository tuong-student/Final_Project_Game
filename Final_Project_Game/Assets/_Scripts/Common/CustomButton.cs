using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Game;
using NOOD;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private Button _button;

    #region UnityFunctions
    void Awake()
    {
        _button = GetComponent<Button>();
    }

    void OnEnable()
    {
        GameInput.onPlayerAccept += PerformClick;
    }
    void OnDisable()
    {
        NoodyCustomCode.UnSubscribeFromStatic(typeof(GameInput), this);
    }
    #endregion

    private void PerformClick()
    {
        if(EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            _button.onClick?.Invoke();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        this.transform.DOScale(1.15f, 0.3f).SetEase(Ease.OutCubic);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        this.transform.DOScale(1, 0.3f).SetEase(Ease.InCubic);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = this.gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }
}
