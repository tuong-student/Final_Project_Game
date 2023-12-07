using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IconHighlight : MonoBehaviour
{
    public Vector3Int cellPosition;
    private Vector3 targetPosition;
    [SerializeField] private Tilemap targetTilemap;
    private SpriteRenderer spriteRenderer;

    private bool canSelect;
    private bool show;

    public bool CanSelect
    {
        set
        {
            canSelect = value;
            gameObject.SetActive(canSelect && show);
        }
    }
    public bool Show
    {
        set
        {
            show = value;
            gameObject.SetActive(canSelect && show);
        }
    }

    private void Update()
    {
        targetPosition = targetTilemap.CellToWorld(cellPosition);
        transform.position = targetPosition + targetTilemap.cellSize / 2;
    }

    public void SetIcon(Sprite icon)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = icon;
    }
}
