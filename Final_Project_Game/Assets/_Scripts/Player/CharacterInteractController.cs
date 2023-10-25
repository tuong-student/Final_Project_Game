using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System;

public class CharacterInteractController : MonoBehaviour
{
    //Transform player;
    Rigidbody2D rgbd2d;
    ToolbarController toolbarController;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1f;
    private Character character;
    private Vector2 positionPlayer = Vector2.zero;
    [SerializeReference] HightlightController hightlight;
    //[SerializeField] MarkerManager markerManager;
    //[SerializeField] TileMapReadController tileMapReadController;
    //[SerializeField] float maxDistance = 1.5f;
    //[SerializeField] CropsManager cropsManager;
    //[SerializeField] TileData plowableTiles;
    //private Vector3Int selectedTilePosition;
    //private bool selectable;

    private void Awake()
    {
        //player = GameManager.instance.player.transform;
        rgbd2d = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
        toolbarController = GetComponent<ToolbarController>();
    }
    private void Update()
    {
        Check();
        if (Input.GetMouseButtonDown(1))
        {
            Interact();
        }
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


    private void Interact()
    {

        Vector2 position = rgbd2d.position * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);
        foreach (Collider2D c in colliders)
        {
            Interactable hit = c.GetComponent<Interactable>();
            if (hit != null)
            {
                hit.Interact(character);
                break;
            }
        }
    }
}
