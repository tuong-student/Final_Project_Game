using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(OptionLogic))]
public class MenuElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public OptionLogic _optionLogic;

    void Awake()
    {
        _optionLogic = GetComponent<OptionLogic>();
    }

    public void Select()
    {
        this.transform.DOScale(1.15f, 0.3f).SetEase(Ease.OutBounce);
    }
    public void Deselect()
    {
        this.transform.DOScale(1, 0.3f).SetEase(Ease.InBounce);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = this.gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = this.gameObject;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Deselect();
    }
}
