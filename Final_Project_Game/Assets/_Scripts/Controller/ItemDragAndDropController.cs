using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDropController : MonoBehaviour
{
    public ItemSlot itemSlot;
    [SerializeField] GameObject ItemIcon;
    RectTransform iconTransform;
    Image itemIconImage;


    private void Start()
    {
        itemSlot = new ItemSlot();
        iconTransform = ItemIcon.GetComponent<RectTransform>();
        itemIconImage = ItemIcon.GetComponent<Image>();
    }

    internal void OnClick(ItemSlot itemSlot)
    {
        if (this.itemSlot.storable == null)
        {
            this.itemSlot.Copy(itemSlot);
            itemSlot.Clear();
        }
        else
        {
            if(itemSlot.storable == this.itemSlot.storable)
            {
                itemSlot.count += this.itemSlot.count;
                this.itemSlot.Clear();
            }
            else
            {
                Storable item = itemSlot.storable;
                int count = itemSlot.count;

                itemSlot.Copy(this.itemSlot);
                this.itemSlot.Set(item, count);
            }
        }
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (itemSlot.storable == null)
        {
            ItemIcon.SetActive(false);
        }
        else
        {
            ItemIcon.SetActive(true);
            itemIconImage.sprite = itemSlot.storable.Icon;
        }
    }


    private void Update()
    {
        if (ItemIcon.activeInHierarchy)
        {
            iconTransform.position = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    worldPosition.z = 0;
                    ItemSpawnManager.instance.SpawnItem(
                        worldPosition,
                        null,
                        itemSlot.storable,
                        itemSlot.count);
                    itemSlot.Clear();
                    ItemIcon.SetActive(false);
                }
            }
        }
    }

    public bool CheckItem(ItemSO item, int count = 1)
    {
        if (itemSlot == null)
            return false;
        if (item.stackable)
        {
            return itemSlot.storable == item && itemSlot.count >= count;
        }

        return itemSlot.storable == item;
    }

    public void RemoveItem(int count = 1)
    {
        if (itemSlot == null)
            return;
        if (itemSlot.storable.Stackable)
        {
            itemSlot.count -= count;
            if (itemSlot.count <= 0)
                itemSlot.Clear();
        }
        else
            itemSlot.Clear();
        UpdateIcon();
    }
}
