using Game;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarController : MonoBehaviour
{
    [SerializeField] int toolbarSize = 12;
    private int selectedTool;
    public Action<int> onChange;
    [SerializeField] IconHightlight iconHightlight;

    public ItemSlot GetItemSlot
    {
        get
        {
            return PlayerManager.Instance.inventoryContainer.slots[selectedTool];
        }
    }
    public Storable GetPlayerSelected
    {
        get
        {
            return PlayerManager.Instance.inventoryContainer.slots[selectedTool].storable;
        }
    }
    private void Start()
    {
        onChange += UpdateHightlightIcon;
        UpdateHightlightIcon(selectedTool);
    }

    private void Update()
    {
        float delta = Input.mouseScrollDelta.y;
        if (delta != 0)
        {
            if (delta > 0)
            {
                selectedTool += 1;
                selectedTool = (selectedTool >= toolbarSize ? 0 : selectedTool);
            }
            else
            {
                selectedTool -= 1;
                selectedTool = (selectedTool < 0 ? toolbarSize - 1 : selectedTool);

            }
            onChange?.Invoke(selectedTool);
        }
    }
    public void Set(int id)
    {
        selectedTool = id;
    }
    public void UpdateHightlightIcon(int id = 0)
    {
        Storable storable = GetPlayerSelected;
        if(storable == null) return;
        if(storable.StorageType == StorageType.FarmItem)
        {
            Item item = storable as Item;
            if (item == null)
            {
                iconHightlight.Show = false;
                return;
            }

            iconHightlight.Show = item.iconHightlight;
            if (item.iconHightlight)
            {
                iconHightlight.SetIcon(item.icon);
            }
            PlayerManager.Instance.ChangeGun(null);
        }
        if(storable.StorageType == StorageType.Weapon)
        {
            PlayerManager.Instance.ChangeGun((GunSO)storable);
        }
    }
}
