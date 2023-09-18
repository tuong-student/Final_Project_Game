using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class CharacterInteractController : MonoBehaviour
{
    //Transform player;
    Rigidbody2D rgbd2d;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1f;
    private Character character;
    private Vector2 positionPlayer = Vector2.zero;
    [SerializeReference] HightlightController hightlight;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController tileMapReadController;
    [SerializeField] float maxDistance = 1.5f;
    [SerializeField] CropsManager cropsManager;
    [SerializeField] TileData plowableTiles;
    private Vector3Int selectedTilePosition;
    private bool selectable;

    private void Awake()
    {
        //player = GameManager.instance.player.transform;
        rgbd2d = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();

    }
    private void Update()
    {
        SelectTile();
        CanSelectCheck();
        Marker();
        Check();
        if (Input.GetMouseButtonDown(1))
        {
            if(Interact()) return;
            UseToolGrid();
        }
        //positionPlayer.x = player.transform.position.x;
        //positionPlayer.y = player.transform.position.y;
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

    private void Check()
    {
        
        Vector2 position = rgbd2d.position * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);
        foreach (Collider2D c in colliders)
        {
            Interactable hit = c.GetComponent<Interactable>();
            if (hit != null)
            {
                hightlight.Hightlight(hit.gameObject);
                return;
            }
        }
        hightlight.Hide();
    }
    private bool Interact()
    {
        Vector2 position = rgbd2d.position * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);
        foreach (Collider2D c in colliders)
        {
            Interactable hit = c.GetComponent<Interactable>();
            if(hit != null)
            {
                hit.Interact(character);
                return true;
            }
        }
        return false;
    }
    private void UseToolGrid()
    {
        if (selectable == true)
        {
            TileBase tileBase = tileMapReadController.GetTileBase(selectedTilePosition);
            TileData tileData = tileMapReadController.GetTileData(tileBase);
            if (tileData != plowableTiles) return;
            if (cropsManager.Check(selectedTilePosition))
                cropsManager.Seed(selectedTilePosition);
            else
                cropsManager.Plow(selectedTilePosition);
        }
    }
}
