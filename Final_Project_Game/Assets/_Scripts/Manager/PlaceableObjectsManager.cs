using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceableObjectsManager : MonoBehaviour
{
    [SerializeField] PlaceableObjectsContainer placeObjects;
    [SerializeField] Tilemap targetTilemap;

    private void Start()
    {
        GameManager.instance.GetComponent<PlaceableObjectReferenceManager>().placeableObjectsManager = this;
    }
    public void PlaceObject(Item item, Vector3Int positionOnGrid)
    {
        GameObject go = Instantiate(item.itemPrefab);
        Vector3 position = targetTilemap.CellToWorld(positionOnGrid) + targetTilemap.cellSize / 2;
        go.transform.position = position;
        placeObjects.placeaableObjects.Add(new PlaceableObject(item,go.transform,positionOnGrid));
    }
}
