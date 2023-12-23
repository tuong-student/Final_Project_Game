using System.Linq;
using DG.Tweening;
using Game;
using NOOD;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelGroup : MonoBehaviorInstance<PanelGroup>
{
    // This is crafting panel group
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private CanvasGroup _canvasGroup;
    public void Open()
    {
        inventoryPanel.SetActive(true);
        this.gameObject.transform.DOScale(1, 1f);
        _canvasGroup.DOFade(1, 0.7f);
        _canvasGroup.interactable = true;
    }
    public void Close()
    {
        this.gameObject.transform.DOScale(0.3f, 1f);
        _canvasGroup.DOFade(0, 0.7f).OnComplete(() =>
        {
            _canvasGroup.interactable = false;
            inventoryPanel.gameObject.SetActive(false);
        });
    }

}
