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
    level4,
    level5
}

[RequireComponent(typeof(TimeAgent))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<BaseEnemy> _enemyListToSpawn = new List<BaseEnemy>();
    [SerializeField] private List<Transform> _enemySpawnPosition = new List<Transform>();

    [SerializeField] private Difficulty _difficulty = Difficulty.level1;

    public void ChangeDifficulty(Difficulty difficulty)
    {
        _difficulty = difficulty;
    }

    public void SpawnRandomEnemy(int number)
    {
        for(int i = 0; i < number + (int)_difficulty; i++)
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
