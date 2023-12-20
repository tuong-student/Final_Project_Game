using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerInteractController : MonoBehaviour
{
    private ItemContainer targetItemContainer;
    private InventoryController inventoryController;
    [SerializeField] private ItemContainerPanel itemContainerPanel;
    [SerializeField] private Transform openedChest;
    [SerializeField] private float maxDistance = 1.5f;

    private void Awake()
    {
        inventoryController = GetComponent<InventoryController>();
    }

    private void Update()
    {
        if (openedChest != null)
        {
            float distance = Vector2.Distance(openedChest.position, transform.position);
            {
                if (distance > maxDistance)
                {
                    openedChest.GetComponent<LootContainerInteract>().Close(GetComponent<Character>());
                }
            }
        }
    }

    public void Open(ItemContainer itemContainer,Transform _openedChest)
    {
        targetItemContainer = itemContainer;
        itemContainerPanel.inventory = targetItemContainer;
        inventoryController.Open();
        inventoryController.panel.transform.localPosition = new Vector3(0, -211.29f, 0);
        itemContainerPanel.gameObject.SetActive(true);
        openedChest = _openedChest;
    }

    public void Close()
    {
        inventoryController.panel.transform.localPosition = new Vector3(0, -19.4152f, 0);
        inventoryController.Close();
        itemContainerPanel.gameObject.SetActive(false);
        openedChest = null;
    }
}
