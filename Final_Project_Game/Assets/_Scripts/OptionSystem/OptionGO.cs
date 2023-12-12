using System.Collections;
using System.Collections.Generic;
using Game.Interface;
using NOOD.Sound;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionGO : MonoBehaviour, IPointerEnterHandler, IInteractable
{
    [SerializeField] private IOptionUIBase _parentOptionUI;

    void Awake()
    {
        _parentOptionUI = GetComponentInParent<IOptionUIBase>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _parentOptionUI.SelectOptionObject(this.gameObject);
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.Hover);
    }

    public void Interact(object interactor)
    {
        // Do nothing
    }
}
