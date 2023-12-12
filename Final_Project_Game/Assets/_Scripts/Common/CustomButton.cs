using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Game;
using NOOD;
using System;
using NOOD.Sound;

[RequireComponent(typeof(Button))]
public class CustomButton : InteractableUIBase, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    private Button _button;

    #region UnityFunctions
    void Awake()
    {
        _button = GetComponent<Button>();
    }
    void OnEnable()
    {
        // GameInput.onPlayerAccept += TryClick;
    }
    void OnDisable()
    {
        NoodyCustomCode.UnSubscribeFromStatic(typeof(GameInput), this);
    }
    #endregion

    #region Perform functions
    private void TryClick()
    {
        if(EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            _button.onClick?.Invoke();
        }
    }
    public void SetAction(Action action)
    {
        _button.onClick.AddListener(() => action?.Invoke());
    }
    #endregion

    #region Interface functions
    public void OnSelect(BaseEventData eventData)
    {
        this.transform.DOScale(1.15f, 0.3f).SetEase(Ease.OutCubic);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        this.transform.DOScale(1, 0.3f).SetEase(Ease.InCubic);
    }
    #endregion

    #region Override functions
    public override void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = this.gameObject;
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
        base.OnPointerExit(eventData);
    }
    public override void Interact(object sender)
    {
        // Do nothing
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(GlobalConfig._isSoundMute == false)
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
    }
    #endregion
}
