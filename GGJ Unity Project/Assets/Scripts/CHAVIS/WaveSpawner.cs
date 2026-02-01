using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CHAVIS
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private float countdown;
        [SerializeField] private List<GameObject> spawnPoints;
        [SerializeField] private Transform spawnPointRoot;


        public PowerUpManager powerUpManager;

        public Wave[] waves;
        public int currentWaveIndex = 0;

        private bool readyToCountDown;
        private bool loggedMissingConfig;
        private bool _waveStingerPlayed;

        public float enemyInsetDefault = 1.5f;
        private void Awake()
        {
            
        }
        private void Start()
        {
            readyToCountDown = true;

            InitializeSpawnPoints();

            if (waves == null || waves.Length == 0)
            {
                LogMissingConfig("No waves are assigned to WaveSpawner.");
                return;
            }

            for (int i = 0; i < waves.Length; i++)
            {
                waves[i].enemiesLeft = waves[i].enemies != null ? waves[i].enemies.Length : 0;
            }

            if (countdown <= 0f)
            {
                countdown = waves[currentWaveIndex].timeToNextWave;
            }
        }
        private void Update()
        {
            if (waves == null || waves.Length == 0)
            {
                LogMissingConfig("No waves are assigned to WaveSpawner.");
                return;
            }

            if (currentWaveIndex >= waves.Length)
            {
                //Debug.Log("You survived every wave!");
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

                if (!_waveStingerPlayed)
                {
                    AudioManager.Instance?.PlayWaveStinger(currentWaveIndex + 1);
                    _waveStingerPlayed = true;
                }

                if (spawnPoints != null && spawnPoints.Count > 0 && waves[currentWaveIndex].enemiesLeft > 0)
                {
                    StartCoroutine(SpawnWave());
                }
                else if (spawnPoints == null || spawnPoints.Count == 0)
                {
                    LogMissingConfig("No spawn points are assigned to WaveSpawner.");
                }
            }

            if (waves[currentWaveIndex].enemiesLeft <= 0)
            {
                WaveEnd();
                readyToCountDown = true;
                _waveStingerPlayed = false;

                currentWaveIndex += 1;
            }
        }
        private IEnumerator SpawnWave()
        {
            if (spawnPoints == null || spawnPoints.Count == 0)
            {
                yield break;
            }

            int x = spawnPoints.Count;
            if (currentWaveIndex < waves.Length)
            {
                var enemies = waves[currentWaveIndex].enemies;
                if (enemies == null || enemies.Length == 0)
                {
                    LogMissingConfig("Wave has no enemies assigned.");
                    yield break;
                }

                for (int i = 0; i < enemies.Length; i++)
                {
                    GameObject spawnPoint = spawnPoints[Random.Range(0, x)];
                    if (spawnPoint == null || enemies[i] == null)
                    {
                        continue;
                    }

                    GameObject enemy = Instantiate(enemies[i], spawnPoint.transform);

                    enemy.transform.SetParent(spawnPoint.transform);

                    yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
                }
            }
        }
        public void WaveEnd()
        {

            Time.timeScale = 0f;
            if (powerUpManager != null)
            {
                powerUpManager.RandomizeNewPowerUps();
            }
            if (InGameMenus.Instance != null)
            {
                InGameMenus.Instance.ChangeState(InGameMenus.GameState.PowerUpSelection);
            }

        }

        private void InitializeSpawnPoints()
        {
            if (spawnPoints == null)
            {
                spawnPoints = new List<GameObject>();
            }

            spawnPoints.RemoveAll(point => point == null);

            if (spawnPoints.Count > 0)
            {
                return;
            }

            Transform root = spawnPointRoot != null ? spawnPointRoot : transform;
            Transform[] children = root.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] != root)
                {
                    spawnPoints.Add(children[i].gameObject);
                }
            }

            spawnPoints.RemoveAll(point => point == null);
        }

        private void LogMissingConfig(string message)
        {
            if (loggedMissingConfig)
            {
                return;
            }

            loggedMissingConfig = true;
            Debug.LogWarning(message, this);
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
}
