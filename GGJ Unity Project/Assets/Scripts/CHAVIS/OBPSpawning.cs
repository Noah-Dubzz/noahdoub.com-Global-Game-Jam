using CHAVIS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Pool;

public class OBPSpawning : MonoBehaviour
{
    public int poolSize = 20;
    public int enemiesThisWave = 50;
    
    [SerializeField] public ObjectPool[] enemyPrefab;
    private ObjectPool<ObjectPool> enemyPool1;
    private ObjectPool<ObjectPool> enemyPool2;
    private ObjectPool<ObjectPool> enemyPool3;
    [SerializeField] private List<GameObject> spawnPoints;

    public static OBPSpawning Instance;

    public float spawnDelay = 1.0f;
    private float _spawnTimer;
    private bool _isPrewarming = false;
    private bool _isRunning = true;
    private int _enemiesLeftToKill;

    private void Awake()
    {
        Instance = this;
        if (enemyPrefab.Length >= 1)
            enemyPool1 = new ObjectPool<ObjectPool>(CreateEnemy1, OnGet, OnRelease);
        
        if (enemyPrefab.Length >= 2)
            enemyPool2 = new ObjectPool<ObjectPool>(CreateEnemy2, OnGet, OnRelease);

        if (enemyPrefab.Length >= 3)
            enemyPool3 = new ObjectPool<ObjectPool>(CreateEnemy3, OnGet, OnRelease);
    }

    public ObjectPool CreateEnemy1()
    {
        ObjectPool enemy = Instantiate(enemyPrefab[0]);
        enemy.SetPool(enemyPool1);
        return enemy;
    }

    public ObjectPool CreateEnemy2()
    {
        ObjectPool enemy = Instantiate(enemyPrefab[1]);
        enemy.SetPool(enemyPool2);
        return enemy;
    }

    public ObjectPool CreateEnemy3()
    {
        ObjectPool enemy = Instantiate(enemyPrefab[2]);
        enemy.SetPool(enemyPool3);
        return enemy;
    }

    private void OnGet(ObjectPool enemy)
    {
        enemy.gameObject.SetActive(true);
        //Spawn(enemy);
        
    }
    private void OnRelease(ObjectPool enemy)
    {
        enemy.gameObject.SetActive(false);
        if (!_isPrewarming)
        {
            enemiesThisWave--;
        }
    }
    public void Spawn(ObjectPool enemy)
    {
        int x = spawnPoints.Count;
        GameObject spawnPoint = spawnPoints[Random.Range(0, x)];
        Transform randomSpawnPoint = spawnPoint.transform;
        enemy.transform.position = randomSpawnPoint.position;
        
        
    }

    public PowerUpManager powerUpManager;
    

    private void Start()
    {
        PrepareWave();
    }
    private void Update()
    {
        if(_isRunning) {
            if (enemiesThisWave <= 0)
            {
                //Debug.Log("You survived every wave!");
                //SceneManager.LoadScene("Win");
                
                Debug.Log("WAVE FINISHED");
                _isRunning = false;
                InGameMenus.Instance.ChangeState(InGameMenus.GameState.PowerUpSelection);
                return;
            }

            _spawnTimer += Time.deltaTime;
            int currentActive = enemyPool1.CountActive + enemyPool2.CountActive + enemyPool3.CountActive;
            if (currentActive < poolSize && _spawnTimer >= spawnDelay && currentActive != _enemiesLeftToKill)
            {
                ActivateEnemy();
                _enemiesLeftToKill--;
                _spawnTimer = 0f;
            }
        }
    }

    public void ActivateEnemy()
    {
        float r = Random.Range(0f, 1f);
        ObjectPool enemy = null;
        switch (r)
        {
            case < .45f:
                enemy = enemyPool1.Get();
                break;
            case < .75f:
                enemy = enemyPool2.Get();
                break;
            case >= .75f:
                enemy = enemyPool3.Get();
                break;
            default:
                enemy = enemyPool1.Get();
                break;
        }
        
        if (enemy != null)
        {
            Spawn(enemy);
        }
    }

    public void PrepareWave()
    {
        _isRunning = true;
        _enemiesLeftToKill = enemiesThisWave;
        PrewarmPools();
    }
    
    public void ResetPools()
    {
        ObjectPool[] allEnemies = FindObjectsByType<ObjectPool>(FindObjectsSortMode.None);
        foreach (ObjectPool enemy in allEnemies)
        {
            Destroy(enemy.gameObject);
        }
        
        enemyPool1.Clear();
        enemyPool2.Clear();
        enemyPool3.Clear();
        
        PrewarmPools(); 
    }
    
    private void PrewarmPools()
    {
        _isPrewarming = true;
        List<ObjectPool> temp = new List<ObjectPool>();

        if (enemyPrefab.Length >= 1)
        {
            //Prewarm Pool 1
            for (int i = 0; i < poolSize; i++)
            {
                temp.Add(enemyPool1.Get());
            }

            foreach (var enemy in temp) enemyPool1.Release(enemy);
            temp.Clear();
        }

        if (enemyPrefab.Length >= 2)
        {
            //Prewarm Pool 2
            for (int i = 0; i < poolSize; i++) temp.Add(enemyPool2.Get());
            foreach (var enemy in temp) enemyPool2.Release(enemy);
            temp.Clear();
        }

        if (enemyPrefab.Length >= 3)
        {
            //Prewarm Pool 3
            for (int i = 0; i < poolSize; i++) temp.Add(enemyPool3.Get());
            foreach (var enemy in temp) enemyPool3.Release(enemy);
        }

        _isPrewarming = false;
    }

}
