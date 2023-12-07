using Game;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class ToolbarController : MonoBehaviour
{
    [SerializeField] int toolbarSize = 12;
    private int selectedTool;
    public Action<int> onChange;
    [SerializeField] IconHighlight iconHighlight;

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
        onChange += UpdateHighlightIcon;
        UpdateHighlightIcon(selectedTool);
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
    public void UpdateHighlightIcon(int id = 0)
    {
        Storable storable = GetPlayerSelected;
        if(storable == null) return;
        if(storable.StorageType == StorageType.FarmItem)
        {
            ItemSO item = storable as ItemSO;
            if (item == null)
            {
                iconHighlight.Show = false;
                return;
            }

            iconHighlight.Show = item.iconHighlight;
            if (item.iconHighlight)
            {
                iconHighlight.SetIcon(item.icon);
            }
            PlayerManager.Instance.ChangeItem(item);
        }
        if(storable.StorageType == StorageType.Weapon)
        {
            GunSO gunSO = storable as GunSO;
            PlayerManager.Instance.ChangeItem(gunSO);
        }
    }
}
