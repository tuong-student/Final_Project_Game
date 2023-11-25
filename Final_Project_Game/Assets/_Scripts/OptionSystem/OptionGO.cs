using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionGO : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private OptionUI _parentOptionUI;

    void Awake()
    {
        _parentOptionUI = GetComponentInParent<OptionUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _parentOptionUI.SelectOptionObject(this.gameObject);
    }
}
