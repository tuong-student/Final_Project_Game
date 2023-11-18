using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using DG.Tweening;

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

    public void SpawnManyItem(Vector3 position, Transform parent, Item item, int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(pickupItemPrefab, parent);
            go.transform.localScale = Vector3.zero;
            go.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.3f);
            go.transform.position = NoodyCustomCode.GetPointAroundAPosition2D(position, 0.3f);
            go.GetComponent<PickUpItem>().Set(item, 1);
        }
    }
}
