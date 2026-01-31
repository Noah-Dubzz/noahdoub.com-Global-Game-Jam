using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.LowLevel;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    //public InGameMenus powerUpChoices;
    [SerializeField] private float countdown;
    [SerializeField] private List<GameObject> spawnPoints;
    

    public PowerUpManager powerUpManager;

    public Wave[] waves;
    public int currentWaveIndex = 0;

    private bool readyToCountDown;

    public float enemyInsetDefault = 1.5f;
    private void Awake()
    {

    }
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
            readyToCountDown = false;

            countdown = waves[currentWaveIndex].timeToNextWave;

            StartCoroutine(SpawnWave());
        }

        if (waves[currentWaveIndex].enemiesLeft <= 0)
        {
            WaveEnd();
            readyToCountDown = true;

            currentWaveIndex += 1;
        }
    }
    private IEnumerator SpawnWave()
    {
        int x = spawnPoints.Count;
        if (currentWaveIndex < waves.Length)
        {
            for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)
            {
                GameObject spawnPoint = spawnPoints[Random.Range(0, x)];
                Enemy enemy = Instantiate(waves[currentWaveIndex].enemies[i], spawnPoint.transform);

                enemy.transform.SetParent(spawnPoint.transform);

                yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
            }
        }
    }
    public void WaveEnd()
    {
        
        Time.timeScale = 0f;
        powerUpManager.RandomizeNewPowerUps();
        //InGameMenus.Instance.ChangeState(InGameMenus.GameState.PowerUpSelection);
        
    }
    
}

[System.Serializable]
public class Wave
{
    public Enemy[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
}

