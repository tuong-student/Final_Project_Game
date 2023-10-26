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
            if (placeObjects.placeableObjects[i].targetObject == null)
                continue;
            IPersistant persistant = placeObjects.placeableObjects[i].targetObject.GetComponent<IPersistant>();
            if(persistant !=null)
            {
                string jsonString = persistant.Read();
                placeObjects.placeableObjects[i].objectState = jsonString;
            }
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

    public void PickUp(Vector3Int gridPosition)
    {
        PlaceableObject placedObject = placeObjects.GetPosition(gridPosition);
        if (placedObject == null)
            return;
        ItemSpawnManager.instance.SpawnItem(
            targetTilemap.CellToWorld(gridPosition),
            null,
            placedObject.placedItem,
            1
            );
        Destroy(placedObject.targetObject.gameObject);

        placeObjects.Remove(placedObject);
    }

    private void VisualizeItem(PlaceableObject placeableObject)
    {
        GameObject go = Instantiate(placeableObject.placedItem.itemPrefab);
        go.transform.parent = transform;
        Vector3 position = targetTilemap.CellToWorld(placeableObject.positionOnGrid) 
            + targetTilemap.cellSize / 2;
        go.transform.position = position;
        IPersistant persistant = go.GetComponent<IPersistant>();
        if (persistant != null)
        {
            persistant.Load(placeableObject.objectState);
        }
        placeableObject.targetObject = go.transform;
    }

    public bool IsThisPositionExist(Vector3Int position)
    {
        return placeObjects.GetPosition(position) != null;
    }

    public void PlaceObject(Item item, Vector3Int positionOnGrid)
    {
        if (IsThisPositionExist(positionOnGrid))
            return;
        PlaceableObject placeableObject = new PlaceableObject(item, positionOnGrid);
        VisualizeItem(placeableObject);
        placeObjects.placeableObjects.Add(placeableObject);
    }
}
