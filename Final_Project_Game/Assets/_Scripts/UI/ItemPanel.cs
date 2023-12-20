using System.Collections;
using System.Collections.Generic;
using Game;
using NOOD;
using UnityEngine;

public class ItemPanel : MonoBehaviour
{
    public ItemContainer inventory;
    public List<InventoryButton> buttons;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        SetButtonIndex();
        InitButtons();
    }

    private void SetButtonIndex()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].SetIndex(index);
        }
    }
    private void OnEnable()
    {
        InitButtons();
        PlayerManager.Instance.OnPlayerInventoryChange += InitButtons;
    }
    void OnDisable()
    {
        NoodyCustomCode.UnSubscribeAllEvent(PlayerManager.Instance, this);
    }

    private void LateUpdate()
    {
        if (inventory.isDirty)
        {
            InitButtons();
            inventory.isDirty = false; 
        }
    }
    public virtual void InitButtons()
    {
        for (int i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
        {
            if (inventory.slots[i].storable == null)
            {
                buttons[i].Clean();
            }
            else
            {
                buttons[i].SetItem(inventory.slots[i]);
            }
        }
    }
    public virtual void OnClick(int id)
    {
        // Override by children
    }
}
