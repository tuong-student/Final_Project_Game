using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemConvertorData
{
    public  ItemSlot itemSlot;
    public  int timer;
    public ItemConvertorData()
    {
        itemSlot = new ItemSlot();
    }

}
[RequireComponent(typeof(TimeAgent))]
public class ItemConvertorInteract : Interactable, IPersistant
{
    [SerializeField] private ItemSO convertableItem;
    [SerializeField] private ItemSO producedItem;
    [SerializeField] private int producedItemCount = 1;
    [SerializeField] private int timetoProcess = 5;
    private ItemConvertorData data;
    private Animator anim;

    private void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += ItemConvertProcess;
        if (data == null)
        {
            data = new ItemConvertorData();
        }
        anim = GetComponent<Animator>();
        Animate();
    }

    public override void Interact(Character character)
    {
        if (data.itemSlot.storable == null)
        {
            if (GameManager.instance.dragAndDropController.CheckItem(convertableItem))
            {
                StartItemProcessing(GameManager.instance.dragAndDropController.itemSlot);
                return;
            }
            ToolbarController toolbar = character.GetComponent<ToolbarController>();
            if (toolbar == null)
                return;
            ItemSlot itemSlot = toolbar.GetItemSlot;
            if (itemSlot.storable == convertableItem)
            {
                StartItemProcessing(itemSlot);
            }
        }
        if(data.itemSlot.storable != null && data.timer <= 0f)
        {
            PlayerManager.Instance.InventoryContainer.Add(data.itemSlot.storable, data.itemSlot.count);
            data.itemSlot.Clear();
        }
    }

    private void StartItemProcessing(ItemSlot toProcess)
    {
        data.itemSlot.Copy(GameManager.instance.dragAndDropController.itemSlot);
        data.itemSlot.count = 1;
        if (toProcess.storable.Stackable)
        {
            toProcess.count -= 1;
            if(toProcess.count <= 0)
            {
                toProcess.Clear();
            }
        }
        else
        {
            toProcess.Clear();
        }
        data.timer = timetoProcess;
        Animate();
    }

    private void Animate()
    {
        anim.SetBool("Working", data.timer > 0f);
    }

    private void CompleteItemConversion()
    {
        Animate();
        data.itemSlot.Clear();
        data.itemSlot.Set(producedItem, producedItemCount);
    }

    private void ItemConvertProcess()
    {
        if (data.itemSlot == null)
            return;
        if (data.timer > 0)
        {
            data.timer -= 1;
            if (data.timer <= 0)
            {
                CompleteItemConversion();
            }
        }
    }

    public string Read()
    {
       return JsonUtility.ToJson(data);
    }

    public void Load(string jsonString)
    {
        data = JsonUtility.FromJson<ItemConvertorData>(jsonString);
    }
}
