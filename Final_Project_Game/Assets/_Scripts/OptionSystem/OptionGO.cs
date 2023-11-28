using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionGO : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private IOptionUIBase _parentOptionUI;

    void Awake()
    {
        _parentOptionUI = GetComponentInParent<IOptionUIBase>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _parentOptionUI.SelectOptionObject(this.gameObject);
    }
}
