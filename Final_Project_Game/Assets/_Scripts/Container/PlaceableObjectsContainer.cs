using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlaceableObject
{
    public ItemSO placedItem;
    public Transform targetObject;
    public Vector3Int positionOnGrid;
    /// <summary>
    /// serialized JSON string which contains the state of the object
    /// </summary>
    public string objectState;
    public PlaceableObject(ItemSO item, Vector3Int pos)
    {
        placedItem = item;
        positionOnGrid = pos;
    }
}
[CreateAssetMenu(menuName ="Data/Placeable Objects Container")]
public class PlaceableObjectsContainer : ScriptableObject
{
    public List<PlaceableObject> placeableObjects;

    public PlaceableObject GetPosition(Vector3Int position)
    {
        return placeableObjects.Find(x => x.positionOnGrid == position);
    }

    public void Remove(PlaceableObject placedObject)
    {
        placeableObjects.Remove(placedObject);
    }
    public void ClearAllObj()
    {
        for (int i = 0; i < placeableObjects.Count; i++)
        {
            placeableObjects.Clear();
        }
    }
}
