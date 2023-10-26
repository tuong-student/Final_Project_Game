using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemConvertorData
{
    public  ItemSlot itemSlot;
    public  float timer;
    public ItemConvertorData()
    {
        itemSlot = new ItemSlot();
    }

}

public class ItemConvertorInteract : Interactable, IPersistant
{
    [SerializeField] private Item convertableItem;
    [SerializeField] private Item producedItem;
    [SerializeField] private int producedItemCount = 1;
    [SerializeField] private float timetoProcess = 5f;
    private ItemConvertorData data;
    private Animator anim;

    private void Start()
    {
        if (data == null)
        {
            data = new ItemConvertorData();
        }
        anim = GetComponent<Animator>();
    }

    public override void Interact(Character character)
    {
        if (data.itemSlot.item == null)
        {
            if (GameManager.instance.dragAndDropController.CheckItem(convertableItem))
                StartItemProcessing();
        }
        if(data.itemSlot.item != null && data.timer < 0f)
        {
            GameManager.instance.inventoryContainer.Add(data.itemSlot.item, data.itemSlot.count);
            data.itemSlot.Clear();
        }
    }

    private void StartItemProcessing()
    {
        anim.SetBool("Working", true);
        data.itemSlot.Copy(GameManager.instance.dragAndDropController.itemSlot);
        data.itemSlot.count = 1;
        GameManager.instance.dragAndDropController.RemoveItem();

        data.timer = timetoProcess;
    }

    private void Update()
    {
        if (data.itemSlot == null)
            return;
        if (data.timer > 0f)
        {
            data.timer -= Time.deltaTime;
            if(data.timer <= 0f)
            {
                CompleteItemConversion();
            }
        }
    }

    private void CompleteItemConversion()
    {
        anim.SetBool("Working", false);
        data.itemSlot.Clear();
        data.itemSlot.Set(producedItem, producedItemCount);
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
