using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsCharacterController : MonoBehaviour
{
    CharacterController character;
    Rigidbody2D rgbd2d;
    [SerializeField] ToolbarController toolbarController;
    [SerializeField] private float offsetDistance = 1f;
    [SerializeField] private float sizeOfInteractableArea = 1.2f;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController tileMapReadController;
    [SerializeField] float maxDistance = 1.5f;
    [SerializeField] ToolAction onTilePickUp;
    [SerializeField] IconHighlight iconHightlight;
    //[SerializeField] CropsManager cropsManager;
    //[SerializeField] TileData plowableTiles;

    private Vector3Int selectedTilePosition;
    private bool selectable;

    private void Start()
    {
        character = GetComponent<CharacterController>();
        rgbd2d = GetComponent<Rigidbody2D>();
        toolbarController = GetComponent<ToolbarController>();
    }


    private void Update()
    {
        SelectTile();
        CanSelectCheck();
        Marker();
        if (Input.GetMouseButtonDown(0))
        {
            if (UseToolWorld() == true) return;
            UseToolGrid();
        }   
    }
    private void SelectTile()
    {
        selectedTilePosition = tileMapReadController.GetGridPosition(Input.mousePosition, true);
    }
    private void CanSelectCheck()
    {
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        markerManager.Show(selectable);
        iconHightlight.CanSelect = selectable;
    }
    private void Marker()
    {
        selectedTilePosition.z = 0;
        markerManager.markedCellPosition = selectedTilePosition;
        iconHightlight.cellPosition = selectedTilePosition;
    }

    private bool UseToolWorld()
    {
        if (GlobalConfig.s_IsUiOpen == true) return true;

        Vector2 position = rgbd2d.position * offsetDistance;

        Storable storable = toolbarController.GetPlayerSelected;
        if(storable == null) return false;
        if(storable.StorageType != StorageType.FarmItem) return false;

        ItemSO item = (ItemSO) toolbarController.GetPlayerSelected;
        if (item.onAction == null) return false;
        bool complete = item.onAction.OnApply(position);
        if (complete)
        {
            if (item.onItemUsed != null)
            {
                item.onItemUsed.OnItemUsed(item, PlayerManager.Instance.inventoryContainer);
            }
        }
        return complete;
    }

    private void UseToolGrid()
    {
        if (selectable && GlobalConfig.s_IsUiOpen == false)
        {
            Storable storable = toolbarController.GetPlayerSelected;
            if (storable == null)
            {
                PickUpTile();
                return;
            }
            switch (storable.StorageType)
            {
                case StorageType.None:
                    break;
                case StorageType.FarmItem:
                    ItemSO itemSO = storable as ItemSO;
                    if (itemSO.onTileMapAction == null) return;

                    bool complete = itemSO.onTileMapAction.OnApplyTileMap(selectedTilePosition, tileMapReadController, itemSO);
                    if (complete)
                    {
                        if (itemSO.onItemUsed != null)
                        {
                            itemSO.onItemUsed.OnItemUsed(itemSO, PlayerManager.Instance.inventoryContainer);
                        }
                    }
                    break;
                case StorageType.Crop:
                    CropSO cropSO = storable as CropSO;

                    if (cropSO.onTileMapAction == null) return;

                    complete = cropSO.onTileMapAction.OnApplyTileMap(selectedTilePosition, tileMapReadController, cropSO);
                    if (complete)
                    {
                        if (cropSO.onItemUsed != null)
                        {
                            cropSO.onItemUsed.OnItemUsed(cropSO, PlayerManager.Instance.inventoryContainer);
                        }
                    }
                    break;
                case StorageType.Weapon:
                    GunSO gunSO = storable as GunSO;
                    break;
                case StorageType.Object:
                    itemSO = storable as ItemSO;
                    if (itemSO.onTileMapAction == null) return;
                    complete = itemSO.onTileMapAction.OnApplyTileMap(selectedTilePosition, tileMapReadController, itemSO);
                    if (complete)
                    {
                        if (itemSO.onItemUsed != null)
                        {
                            itemSO.onItemUsed.OnItemUsed(itemSO, PlayerManager.Instance.inventoryContainer);
                        }
                    }
                    break;
            }
        }
    }
    private void PickUpTile()
    {
        if (onTilePickUp == null)
            return;
        onTilePickUp.OnApplyTileMap(selectedTilePosition, tileMapReadController, null);
    }
}
