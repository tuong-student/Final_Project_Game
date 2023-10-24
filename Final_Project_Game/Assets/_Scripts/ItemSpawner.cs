using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeAgent))]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] Item toSpawn;
    [SerializeField] int count;
    [SerializeField] private float spread = 2f;
    [SerializeField] private float probability = 0.5f;
    private void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += Spawn;
    }
    private void Spawn()
    {
        if (UnityEngine.Random.value < probability)
        {
            Vector3 position = transform.position;
            position.x += UnityEngine.Random.value - spread / 2;
            position.y += UnityEngine.Random.value - spread / 2;
            ItemSpawnManager.instance.SpawnItem(position, this.transform, toSpawn, count);
        }
    }
}
