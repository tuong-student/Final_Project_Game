using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootContainerInteract : Interactable
{
    [SerializeField] GameObject closeChest;
    [SerializeField] GameObject openChest;
    [SerializeField] bool isOpen;

    public override void Interact(Character character)
    {
        if (!isOpen)
        {
            isOpen = true;
            closeChest.SetActive(false);
            openChest.SetActive(true);
        }
    }
}
