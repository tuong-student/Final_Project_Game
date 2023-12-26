using Game;
using NOOD;
using NOOD.SerializableDictionary;
using NOOD.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    #region SerializeField
    [Header("Default Items")]
    [SerializeField] private SerializableDictionary<Storable, int> _defaultItems = new SerializableDictionary<Storable, int>();
    #endregion

    #region static
    public static GameManager instance;
    #endregion

    #region public
    [Space(10)]
    public GameObject player;
    public ItemDragAndDropController dragAndDropController;
    public DayTimeController timeController;
    public DialogueSystem dialogueSystem;
    public ItemList itemDB;
    public GameStatusSO gameStatus;
    public PlaceableObjectsContainer placeableObjectsContainer;
    #endregion

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
    }
    void OnDisable()
    {
        GameInput.Dispose();
    }
    #endregion

    public Transform GetTransform()
    {
        return player.transform;
    }

    public void NewGame()
    {
        player.transform.position = new Vector3(-9.93f, 10.68f, 0f);
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        playerManager.ClearAllInventory();
        playerManager.AddDefaultItems(_defaultItems.Dictionary);        
        
        placeableObjectsContainer.ClearAllObj();
        gameStatus.nameScene = "2DMainGame";
        NoodyCustomCode.StartDelayFunction(() => gameStatus.isNewGame = false, 2f);
    }
}
