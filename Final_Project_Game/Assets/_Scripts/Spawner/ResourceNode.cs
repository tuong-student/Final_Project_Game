using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ResourceNode : ToolHit
{
    [SerializeField] GameObject pickUpDrop;
    [SerializeField] float spread = 0.7f;
    [SerializeField] ItemSO item;
    [SerializeField] int itemCountInOneDrop = 0;
    [SerializeField] private int dropCount = 1;
    [SerializeField] int damage = 8;
    [SerializeField] ResourceNodeType nodeType;

    public override void Hit()
    {
        damage--;
        if (damage == 0)
        {
            if (nodeType == ResourceNodeType.Ore)
                itemCountInOneDrop = 1;
            while (dropCount > 0)
            {
                dropCount--;
                itemCountInOneDrop = UnityEngine.Random.Range(1, 3);
                Vector3 position = transform.position;
                position.x += UnityEngine.Random.value - spread / 2;
                position.y += UnityEngine.Random.value - spread / 2;
                //GameObject go = Instantiate(pickUpDrop);
                ItemSpawnManager.instance.SpawnItem(position, null, item, itemCountInOneDrop);
            }
            Destroy(gameObject);
        }
    }

    public override bool CanBeHit(List<ResourceNodeType> canbehit)
    {
        return canbehit.Contains(nodeType);
    }
}

