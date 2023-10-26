using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConvertorInteract : Interactable
{
    [SerializeField] private Item convertableItem;
    [SerializeField] private Item producedItem;
    [SerializeField] private int producedItemCount = 1;

    private ItemSlot itemSlot;

    [SerializeField] private float timetoProcess = 5f;

    private float timer;
    private Animator anim;

    private void Start()
    {
        itemSlot = new ItemSlot();
        anim = GetComponent<Animator>();
    }

    public override void Interact(Character character)
    {
        if (itemSlot.item == null)
        {
            if (GameManager.instance.dragAndDropController.CheckItem(convertableItem))
                StartItemProcessing();
        }
        if(itemSlot.item != null && timer < 0f)
        {
            GameManager.instance.inventoryContainer.Add(itemSlot.item, itemSlot.count);
            itemSlot.Clear();
        }
    }

    private void StartItemProcessing()
    {
        anim.SetBool("Working", true);
        itemSlot.Copy(GameManager.instance.dragAndDropController.itemSlot);
        GameManager.instance.dragAndDropController.RemoveItem();

        timer = timetoProcess;
    }

    private void Update()
    {
        if (itemSlot == null)
            return;
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                CompleteItemConversion();
            }
        }
    }

    private void CompleteItemConversion()
    {
        anim.SetBool("Working", false);
        itemSlot.Clear();
        itemSlot.Set(producedItem, producedItemCount);
    }
}
