using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using System;
using NOOD;

public class CharacterInteractController : MonoBehaviour
{
    //Transform player;
    Rigidbody2D rgbd2d;
    ToolbarController toolbarController;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1f;
    [SerializeReference] HighlightController highlight;
    private Character character;
    private Vector2 positionPlayer = Vector2.zero;

    #region Unity functions
    private void Awake()
    {
        //player = GameManager.instance.player.transform;
        rgbd2d = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
        toolbarController = GetComponent<ToolbarController>();
    }
    void Start()
    {
        GameInput.onPlayerPressInteract += Interact;
    }
    private void Update()
    {
        Check();
    }
    void OnDisable()
    {
        NoodyCustomCode.UnSubscribeFromStatic(typeof(GameInput), this);
    }
    #endregion

    private void Check()
    {
        Vector2 position = rgbd2d.position * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);
        foreach (Collider2D c in colliders)
        {
            Interactable hit = c.GetComponent<Interactable>();
            if (hit != null)
            {
                highlight.Highlight(hit.gameObject);
                return;
            }
        }
        highlight.Hide();
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
