using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObjectReferenceManager : MonoBehaviour
{
    public PlaceableObjectsManager placeableObjectsManager;

    public void PlaceObject(Item item,Vector3Int pos)
    {
        if(placeableObjectsManager == null)
        {
            Debug.LogWarning("no reference detected");
            return;
        }
        placeableObjectsManager.PlaceObject(item, pos);
    }
}
