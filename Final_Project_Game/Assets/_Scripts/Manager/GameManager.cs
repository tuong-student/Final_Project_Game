using Game;
using NOOD.Sound;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public ItemDragAndDropController dragAndDropController;
    public DayTimeController timeController;
    public DialogueSystem dialogueSystem;
    public ItemList itemDB;
    public GameStatus gamestatus;
    public PlaceableObjectsContainer placeableObjectsContainer;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (gamestatus.isNewGame)
        {
            NewGame();
        }
        SoundManager.PlayMusic(NOOD.Sound.MusicEnum.Theme);
    }

    public Transform GetTransform()
    {
        return player.transform;
    }

    public void NewGame()
    {
        player.transform.position = new Vector3(-9.93f, 10.68f, 0f);
        player.GetComponent<PlayerManager>().ClearAllInventory();
        placeableObjectsContainer.ClearAllObj();
        gamestatus.isNewGame = false;
        gamestatus.nameScene = "2DMainGame";
    }
}
