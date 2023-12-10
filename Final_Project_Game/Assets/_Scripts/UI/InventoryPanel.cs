using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class InventoryPanel : ItemPanel
{
    public override void InitButtons()
    {
        base.InitButtons();
    }
    public override void OnClick(int id)
    {
        GameManager.instance.dragAndDropController.OnClick(inventory.slots[id]);
        InitButtons();
    }
}
