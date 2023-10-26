using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarController : MonoBehaviour
{
    [SerializeField] int toolbarSize = 12;
    private int selectedTool;
    public Action<int> onChange;
    [SerializeField] IconHightlight iconHightlight;

    public Item GetItem
    {
        get
        {
            return GameManager.instance.inventoryContainer.slots[selectedTool].item;
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
        Item item = GetItem;
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
    }
}
