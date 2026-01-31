using CHAVIS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Pool;

public class OBPSpawning : MonoBehaviour
{
    public int poolSize;
    [SerializeField] public ObjectPool[] enemyPrefab;
    private IObjectPool<ObjectPool> enemyPool1;
    private IObjectPool<ObjectPool> enemyPool2;
    private IObjectPool<ObjectPool> enemyPool3;
    [SerializeField] private List<GameObject> spawnPoints;

    private void Awake()
    {
        enemyPool1 = new ObjectPool<ObjectPool>(CreateEnemy, OnGet, OnRelease);
        enemyPool2 = new ObjectPool<ObjectPool>(CreateEnemy, OnGet, OnRelease);
        enemyPool3 = new ObjectPool<ObjectPool>(CreateEnemy, OnGet, OnRelease);
    }

    public ObjectPool CreateEnemy()
    {
        enemyPrefab[0].SetPool(enemyPool1);
        return enemyPrefab[0];
    }

    private void OnGet(ObjectPool enemy)
    {
        enemy.gameObject.SetActive(false);
        Spawn(enemy);
    }
    private void OnRelease(ObjectPool enemy)
    {
        enemy.gameObject.SetActive(false);
    }
    public void Spawn(ObjectPool enemy)
    {
        int x = spawnPoints.Count;
        GameObject spawnPoint = spawnPoints[Random.Range(0, x)];
        Transform randomSpawnoint = spawnPoint.transform;
        enemy.transform.position = randomSpawnoint.position;
    }
    [SerializeField] private float countdown;

    public PowerUpManager powerUpManager;

    public Wave[] waves;
    public int currentWaveIndex = 0;

    private bool readyToCountDown;

    public float enemyInsetDefault = 1.5f;
    
    private void Start()
    {
        readyToCountDown = true;

        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
    }
    private void Update()
    {
        if (currentWaveIndex >= waves.Length)
        {
            //Debug.Log("You survived every wave!");
            SceneManager.LoadScene("Win");
            return;
        }

        if (readyToCountDown == true)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0)
        {

        }

        if (waves[currentWaveIndex].enemiesLeft <= 0)
        {
            WaveEnd();
            readyToCountDown = true;

            currentWaveIndex += 1;
        }
    }
    public void WaveEnd()
    {

        Time.timeScale = 0f;
        powerUpManager.RandomizeNewPowerUps();
        InGameMenus.Instance.ChangeState(InGameMenus.GameState.PowerUpSelection);

    }

}

[System.Serializable]
public class Wave
{
    public GameObject[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
}

