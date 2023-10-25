using System;
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
        VisualizeMap();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < placeObjects.placeableObjects.Count; i++)
        {
            placeObjects.placeableObjects[i].targetObject = null;
        }
    }

    public void VisualizeMap()
    {
        for (int i = 0; i < placeObjects.placeableObjects.Count; i++)
        {
            VisualizeItem(placeObjects.placeableObjects[i]);
        }
    }

    private void VisualizeItem(PlaceableObject placeableObject)
    {
        GameObject go = Instantiate(placeableObject.placedItem.itemPrefab);
        Vector3 position = targetTilemap.CellToWorld(placeableObject.positionOnGrid) 
            + targetTilemap.cellSize / 2;
        go.transform.position = position;
        placeableObject.targetObject = go.transform;
    }
    public void PlaceObject(Item item, Vector3Int positionOnGrid)
    {
        PlaceableObject placeableObject = new PlaceableObject(item, positionOnGrid);
        VisualizeItem(placeableObject);
        placeObjects.placeableObjects.Add(placeableObject);
    }
}
