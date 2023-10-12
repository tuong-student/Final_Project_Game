using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static SeawispHunter.RolePlay.Attributes.Samples.Main;

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
    }
    private void Marker()
    {
        selectedTilePosition.z = 0;
        markerManager.markedCellPosition = selectedTilePosition;
    }

    private bool UseToolWorld()
    {
        Vector2 position = rgbd2d.position * offsetDistance;

        Item item = toolbarController.GetItem;
        if (item == null) return false;
        if (item.onAction == null) return false;
        bool complete = item.onAction.OnApply(position);
        if (complete)
        {
            if (item.onItemUsed != null)
            {
                item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer);
            }
        }
        return complete;
    }

    private void UseToolGrid()
    {
        if (selectable)
        {
            //TileBase tileBase = tileMapReadController.GetTileBase(selectedTilePosition);
            //TileData tileData = tileMapReadController.GetTileData(tileBase);
            //if (tileData != plowableTiles) return;
            //if (cropsManager.Check(selectedTilePosition))
            //    cropsManager.Seed(selectedTilePosition);
            //else
            //    cropsManager.Plow(selectedTilePosition);

            Item item = toolbarController.GetItem;
            if (item == null) return;
            if (item.onTileMapAction == null) return;

            bool complete = item.onTileMapAction.OnApplyTileMap(selectedTilePosition, tileMapReadController, item);
            if (complete)
            {
                if (item.onItemUsed != null)
                {
                    item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer);
                }
            }
        }
    }
}
