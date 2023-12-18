using System.Collections;
using System.Collections.Generic;
using NOOD;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : InteractableUIBase, IPointerClickHandler, IDisplayInfo
{
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Image _hightLight;
    private Storable _storableData;
    int myIndex;

    public void SetIndex(int index)
    {
        myIndex = index;
    }

    public void SetItem(ItemSlot slot)
    {
        _icon.gameObject.SetActive(true);
        _icon.sprite = slot.storable.IconImage;

        if (slot.storable.Stackable)
        {
            _text.gameObject.SetActive(true);
            _text.text = slot.count.ToString();
        }
        else
            _text.gameObject.SetActive(false);

        _storableData = slot.storable;
    }
    public void Clean()
    {
        _storableData = null;
        _icon.sprite = null;
        _icon.gameObject.SetActive(false);
        _text.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemPanel itemPanel = transform.parent.GetComponent<ItemPanel>();
        itemPanel.OnClick(myIndex);
    }

   public void Highlight(bool b)
    {
        _hightLight.gameObject.SetActive(b);
    }

    public (string, Color) GetName()
    {
        if(_storableData == null)
        {
            return (null, Color.white);
        }
        return (_storableData.name, NoodyCustomCode.HexToColor("#5A00FF"));
    }

    public override void Interact(object sender)
    {
        // Do nothing
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        // Do nothing
    }
}
