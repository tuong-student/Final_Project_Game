using System.Collections;
using System.Collections.Generic;
using NOOD;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerClickHandler, IDisplayInfo
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image hightLight;
    public Storable _storableData;
    int myIndex;

    public void SetIndex(int index)
    {
        myIndex = index;
    }

    public void SetItem(ItemSlot slot)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = slot.storable.IconImage;

        if (slot.storable.Stackable)
        {
            text.gameObject.SetActive(true);
            text.text = slot.count.ToString();
        }
        else
            text.gameObject.SetActive(false);

        _storableData = slot.storable;
    }
    public void Clean()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemPanel itemPanel = transform.parent.GetComponent<ItemPanel>();
        itemPanel.OnClick(myIndex);
    }

   public void Highlight(bool b)
    {
        hightLight.gameObject.SetActive(b);
    }

    public (string, Color) GetName()
    {
        if(_storableData == null)
        {
            return (null, Color.white);
        }
        return (_storableData.name, NoodyCustomCode.HexToColor("#5A00FF"));
    }
}
