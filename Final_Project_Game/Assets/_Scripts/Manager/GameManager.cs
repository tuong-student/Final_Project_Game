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
    public GameStatusSO gameStatus;
    public PlaceableObjectsContainer placeableObjectsContainer;

    #region Unity functions
    void OnEnable()
    {
        GameInput.Init();
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (gameStatus.isNewGame)
        {
            NewGame();
        }
        SoundManager.PlayMusic(NOOD.Sound.MusicEnum.Theme);
    }
    #endregion

    public Transform GetTransform()
    {
        return player.transform;
    }

    public void NewGame()
    {
        player.transform.position = new Vector3(-9.93f, 10.68f, 0f);
        player.GetComponent<PlayerManager>().ClearAllInventory();
        placeableObjectsContainer.ClearAllObj();
        gameStatus.isNewGame = false;
        gameStatus.nameScene = "2DMainGame";
    }
}
