using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TimeAgent))]
public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private float spawnerArea_height = 1f;
    [SerializeField] private float spawnerArea_width = 1f;

    [SerializeField] private GameObject spawn;
    [SerializeField] private List<GameObject> spawnList;
    [SerializeField] private float probability = 0.1f;
    [SerializeField] private int count;
    private void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += Spawn;
    }

    private void Spawn()
    {
        if (Random.value > probability)
            return;
        if (spawnList.Count == count)
            return;
        GameObject go = Instantiate(spawn);
        Transform t = go.transform;
        Vector3 position = transform.position;
        position.x += Random.Range(-spawnerArea_width, spawnerArea_width);
        position.y += Random.Range(-spawnerArea_height, spawnerArea_height);
        t.position = position;
        if (CheckPosition(t))
        {
            Debug.Log("Here");
            DestroyObject(go);
            return;
        }
        else
            spawnList.Add(go);
    }

    private bool CheckPosition(Transform transform)
    {
        foreach (var go in spawnList)
        {
            if (Vector3.Distance(go.transform.position, transform.position) < 3)
                return true;
        }
        return false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnerArea_width * 2, spawnerArea_height * 2));
    }
}
