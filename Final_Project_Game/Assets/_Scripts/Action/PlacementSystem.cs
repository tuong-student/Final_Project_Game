using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private TileBase tileBase;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadDefaultValue());
            var cellPos = tilemap.WorldToCell(mousePos);
            var tileTransform =
                Matrix4x4.Translate(new Vector3(0.03f, 0.28f, 0)) *
                Matrix4x4.Rotate(Quaternion.Euler(0, 0, Random.Range(-10f, 10f)));

            var tileChangeData = new TileChangeData
            {
                position = cellPos,
                tile = tileBase,
                color = Color.white,
                transform = tileTransform
            };
            tilemap.SetTile(tileChangeData, false);
        }
    }
}
