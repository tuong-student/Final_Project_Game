using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemDragAndDropController dragAndDropController;
    public DayTimeController timeController;
    public DialogueSystem dialogueSystem;

    private void Awake()
    {
        instance = this;
    }

    public Transform GetTransform()
    {
        return player.transform;
    }
}
