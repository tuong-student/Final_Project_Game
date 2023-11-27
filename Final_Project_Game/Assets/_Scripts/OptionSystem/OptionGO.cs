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
        Debug.Log("PointerEnter");
        _parentOptionUI.SelectOptionObject(this.gameObject);
    }
}
