using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image hightLight;
    int myIndex;

    public void SetIndex(int index)
    {
        myIndex = index;
    }

    public void SetItem(ItemSlot slot)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = slot.storable.Icon;

        if (slot.storable.Stackable)
        {
            text.gameObject.SetActive(true);
            text.text = slot.count.ToString();
        }
        else
            text.gameObject.SetActive(false);
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
}
