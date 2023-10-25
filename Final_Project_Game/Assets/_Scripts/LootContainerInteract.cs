using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootContainerInteract : Interactable
{
    [SerializeField] GameObject closeChest;
    [SerializeField] GameObject openChest;
    [SerializeField] bool isOpen;
    [SerializeField] private ItemContainer itemContainer;

    public override void Interact(Character character)
    {
        if (!isOpen)
        {
            Open(character);
        }
        else
        {
            Close(character);
        }
    }

    public void Open(Character character)
    {
        isOpen = true;
        closeChest.SetActive(false);
        openChest.SetActive(true);
        character.GetComponent<ItemContainerInteractController>().Open(itemContainer, transform);
    }

    public void Close(Character character)
    {
        isOpen = false;
        closeChest.SetActive(true);
        openChest.SetActive(false);
        character.GetComponent<ItemContainerInteractController>().Close();
    }
}
