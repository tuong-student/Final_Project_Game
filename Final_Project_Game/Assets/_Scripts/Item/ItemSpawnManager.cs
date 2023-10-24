using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    public static ItemSpawnManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] GameObject pickupItemPrefab;

    public void SpawnItem(Vector3 position, Transform parent, Item item, int count) 
    {
        GameObject o = Instantiate(pickupItemPrefab, parent);
        o.transform.position = position;
        o.GetComponent<PickUpItem>().Set(item, count);
    }
}
