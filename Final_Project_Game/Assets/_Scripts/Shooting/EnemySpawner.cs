using System.Collections;
using System.Collections.Generic;
using IngameDebugConsole;
using NOOD;
using UnityEngine;

public enum EnemyType
{

}

public enum Difficulty
{
    level1,
    level2,
    level3,
    level4
}

[RequireComponent(typeof(TimeAgent))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<BaseEnemy> _enemyListToSpawn = new List<BaseEnemy>();
    [SerializeField] private List<Transform> _enemySpawnPosition = new List<Transform>();

    [SerializeField] private int _hoursToSpawn = 22; // Spawn bse on hour of the day
    [SerializeField] private int _enemyToSpawn = 5;
    [SerializeField] private Difficulty _difficulty = Difficulty.level1;

    #region Events
    private bool _isCompleteSpawning;
    #endregion

    void Start()
    {
        #region DebugCommand
        DebugLogConsole.AddCommand<int>("/SpawnEnemy", "Spawn enemy with the number you added", SpawnRandomEnemy);
        #endregion

        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onDayTime += SpawnEnemyBaseOnHour;
        timeAgent.onDayTime += ResetSpawnEnemyOnHour;
    }

    private void SpawnEnemyBaseOnHour(int hour, int minute)
    {
        if(hour == _hoursToSpawn && _isCompleteSpawning == false)
        {
            int number = _enemyToSpawn + (int)_difficulty;
            SpawnRandomEnemy(number);
            _isCompleteSpawning = true;
        }
    }

    private void ResetSpawnEnemyOnHour(int hour, int minute)
    {
        if(hour > 22)
        {
            _isCompleteSpawning = false;
        }
    }

    public void SpawnRandomEnemy(int number)
    {
        for(int i = 0; i < number; i++)
        {
            int r = Random.Range(0, _enemySpawnPosition.Count);
            Vector3 pos = _enemySpawnPosition[r].position;
            Vector3 spawnPos = NoodyCustomCode.GetPointAroundAPosition2D(pos, 3);
            r = Random.Range(0, _enemyListToSpawn.Count);
            BaseEnemy enemy = _enemyListToSpawn[r];

            Instantiate(enemy, spawnPos, Quaternion.identity);
        }
    }
}
