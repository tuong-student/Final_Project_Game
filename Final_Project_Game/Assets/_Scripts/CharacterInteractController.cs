using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.Timeline;

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

    private void Awake()
    {
        //player = GameManager.instance.player.transform;
        rgbd2d = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();

    }
    private void Update()
    {
        Marker();
        Check();
        if (Input.GetMouseButtonDown(1))
        {
            Interact();
        }
        //positionPlayer.x = player.transform.position.x;
        //positionPlayer.y = player.transform.position.y;
    }

    private void Marker()
    {
        Vector3Int gridPosition = tileMapReadController.GetGridPosition(Input.mousePosition, true);
        gridPosition.z = 0;
        markerManager.markedCellPosition = gridPosition;
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
            if(hit != null)
            {
                hit.Interact(character);
                break;
            }
        }
    }
}
