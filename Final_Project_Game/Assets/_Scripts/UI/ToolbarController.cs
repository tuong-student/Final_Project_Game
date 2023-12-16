using Game;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class ToolbarController : MonoBehaviour
{
    [SerializeField] ItemToolbarPanel itemToolbarPanel;
    int toolbarSize;
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
            return GetItemSlot.storable;
        }
    }
    private void Start()
    {
        onChange += UpdateHighlightIcon;
        UpdateHighlightIcon(selectedTool);
        toolbarSize = itemToolbarPanel.GetToolbarCount();
    }

    private void Update()
    {
        if (GlobalConfig.s_IsUiOpen) return;
        
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
            ItemSO itemSO = storable as ItemSO;
            if (itemSO == null)
            {
                iconHighlight.Show = false;
                return;
            }

            iconHighlight.Show = itemSO.iconHighlight;
            if (itemSO.iconHighlight)
            {
                iconHighlight.SetIcon(itemSO.icon);
            }
        }
        PlayerManager.Instance.ChangeItem((IHoldableItem)storable);
    }
}
