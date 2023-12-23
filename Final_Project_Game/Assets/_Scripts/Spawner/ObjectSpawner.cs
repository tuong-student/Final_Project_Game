using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private List<SpawnedObject> spawnedObjects;

    [SerializeField] private JSONStringList targetSaveJSONList;
    [SerializeField] int idInList = -1;

    private void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += Spawn;
        spawnedObjects = new List<SpawnedObject>();
        LoadData();
    }

    private void Spawn()
    {
        //if (Random.value > probability)
        //    return;
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
            Destroy(go);
            return;
        }
        else
        {
            t.SetParent(transform);
            SpawnedObject spawnedObject = t.gameObject.AddComponent<SpawnedObject>();
            spawnList.Add(go);
            spawnedObjects.Add(spawnedObject);
            spawnedObject.objId = spawnList.Count - 1;
        }
    }

    private bool CheckPosition(Transform transform)
    {
        foreach (var go in spawnList)
        {
            if (go == null) return false;

            float distance = Vector3.Distance(go.transform.position, transform.position);
            if (distance < 3)
                return true;
        }
        return false;
    }

    public void SpawnedObjectDesTroyed(SpawnedObject spawnedObject)
    {
        spawnedObjects.Remove(spawnedObject);
        spawnList.RemoveAt(spawnedObject.objId);
    }

    public class ToSave
    {
        public List<SpawnedObject.SaveSpawnedObjectData> spawnedObjectDatas;

        public ToSave()
        {
            spawnedObjectDatas = new List<SpawnedObject.SaveSpawnedObjectData>();
        }
    }

    public string Read()
    {
        ToSave toSave = new ToSave();
        foreach(var item in spawnedObjects)
        {
            if(item)
            {
                toSave.spawnedObjectDatas.Add(
                    new SpawnedObject.SaveSpawnedObjectData(
                        item.objId,
                        item.transform.position));
            }
        }
        string save = JsonUtility.ToJson(toSave);
        Debug.Log(save);
        return save;
    }
    
    
    public void Load(string json)
    {
        if (json == "" || json == "{}" || json == null)
            return;

        ToSave toload = JsonUtility.FromJson<ToSave>(json);
        for (int i = 0; i < toload.spawnedObjectDatas.Count; i++)
        {
            SpawnedObject.SaveSpawnedObjectData data = toload.spawnedObjectDatas[i];
            GameObject go = Instantiate(spawn);
            go.transform.position = data.worldPosition;
            go.transform.SetParent(transform);
            SpawnedObject so = go.AddComponent<SpawnedObject>();
            so.objId = data.objectId;
            spawnedObjects.Add(so);
            spawnList.Add(go);
        }
    }

    private void OnDestroy()
    {
        SaveData();
    }

    private void SaveData()
    {
        if (!CheckJson())
            return;

        string jsonString = Read();
        targetSaveJSONList.SetString(jsonString, idInList);
    }

    private void LoadData()
    {
        if (!CheckJson())
            return;
        Load(targetSaveJSONList.GetString(idInList));
    }

    private bool CheckJson()
    {
        if (targetSaveJSONList == null)
            return false ;
        if (idInList == -1) 
            return false;
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnerArea_width * 2, spawnerArea_height * 2));
    }


}
