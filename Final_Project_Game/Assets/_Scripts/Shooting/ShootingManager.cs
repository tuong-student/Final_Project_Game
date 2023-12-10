using UnityEngine;
using  NOOD.SerializableDictionary;
using MoreMountains.Feedbacks;
using TMPro;
using NOOD;

[RequireComponent(typeof(TimeAgent))]
public class ShootingManager : MonoBehaviorInstance<ShootingManager>
{
    [SerializeField] private SerializableDictionary<FeedbackType, MMF_Player> _feedbackDic = new SerializableDictionary<FeedbackType, MMF_Player>();
    [SerializeField] private TextMeshProUGUI _dayNumber;
    [SerializeField] private int _hourToSpawnEnemy = 22;
    [SerializeField] private int _enemyToSpawn = 5;
    [HideInInspector] public TilemapCropsManager _tilemapCropsManager;
    private bool _isSpawnEnemy;
    private EnemySpawner _enemySpawner;

    public static ShootingManager Create()
    {
        return Instantiate<ShootingManager>(Resources.Load<ShootingManager>("Prefabs/Manager/ShootingManager.prefab"), null);
    }

    #region Unity functions
    void Awake()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onDayTimer = CheckTimeToSpawnEnemies;
        _enemySpawner = GetComponent<EnemySpawner>();
        _enemySpawner.ChangeDifficulty(Difficulty.level1);
    }
    void Start()
    {
        DayTimeController.Instance.onNextDay += PlayNextDayFB;
        _tilemapCropsManager = FindObjectOfType<TilemapCropsManager>();
    }
    void OnDestroy()
    {
        NoodyCustomCode.UnSubscribeAllEvent(DayTimeController.Instance, this);
    }
    #endregion

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
        Debug.Log("days " + days);
        _dayNumber.text = "DAY " + days.ToString();
        MMF_Player fb = _feedbackDic.Dictionary[FeedbackType.NextDay];
        if(fb != null)
            fb.PlayFeedbacks();
    }
}
