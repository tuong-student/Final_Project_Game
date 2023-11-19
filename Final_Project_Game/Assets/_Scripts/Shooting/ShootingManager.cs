using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  NOOD.SerializableDictionary;
using MoreMountains.Feedbacks;
using TMPro;

[RequireComponent(typeof(TimeAgent))]
public class ShootingManager : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<FeedbackType, MMF_Player> _feedbackDic = new SerializableDictionary<FeedbackType, MMF_Player>();
    
    [SerializeField] private TextMeshProUGUI _dayNumber;
    [SerializeField] private int _hourToSpawnEnemy = 22;
    [SerializeField] private int _enemyToSpawn = 5;
    private bool _isSpawnEnemy;

    private EnemySpawner _enemySpawner;

    void Awake()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onDayTime = CheckTimeToSpawnEnemies;
        _enemySpawner = GetComponent<EnemySpawner>();
        _enemySpawner.ChangeDifficulty(Difficulty.level1);
    }

    void Start()
    {
        DayTimeController.onNextDay += PlayNextDayFB;
    }

    private void CheckTimeToSpawnEnemies(int hours, int minute)
    {
        if(hours == _hourToSpawnEnemy - 1)
        {
            // Warn player
            PlayEnemyWarningFB();
        }
        if(hours == _hourToSpawnEnemy && _isSpawnEnemy == false)
        {
            // SpawnEnemy
            _isSpawnEnemy = true;
            _enemySpawner.SpawnRandomEnemy(_enemyToSpawn);
        }
        if(hours != _hourToSpawnEnemy)
        {
            _isSpawnEnemy = false;
        }
    }

    private void PlayEnemyWarningFB()
    {
        MMF_Player fb = _feedbackDic.Dictionary[FeedbackType.EnemyComing];
        if(fb != null)
            fb.PlayFeedbacks();
    }
    private void PlayNextDayFB(int days)
    {
        _dayNumber.text = "DAY " + days.ToString("0");
        MMF_Player fb = _feedbackDic.Dictionary[FeedbackType.NextDay];
        if(fb != null)
            fb.PlayFeedbacks();
    }
}
