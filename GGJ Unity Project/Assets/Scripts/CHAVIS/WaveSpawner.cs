using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CHAVIS
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private float countdown = 5f;
        [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();
        [SerializeField] private int extraEnemiesPerWave = 1; // how many extra enemies each subsequent wave gets

        public PowerUpManager powerUpManager;
        public Wave[] waves;

        // Wave counter (starts at 1). Allows infinite progression by cycling templates.
        public int currentWaveNumber = 1;

        private bool readyToCountDown;
        public float enemyInsetDefault = 1.5f;

        private void Start()
        {
            readyToCountDown = true;

            if (waves != null && waves.Length > 0)
            {
                // ensure templates have a sensible initial enemiesLeft (not used for spawn count directly)
                for (int i = 0; i < waves.Length; i++)
                {
                    waves[i].enemiesLeft = waves[i].enemies != null ? waves[i].enemies.Length : 0;
                }

                // if countdown hasn't been configured in inspector, use first wave's time
                if (countdown <= 0f)
                    countdown = waves[0].timeToNextWave;
            }
            else
            {
                Debug.LogWarning("WaveSpawner: No waves defined.");
            }
        }

        private void Update()
        {
            if (waves == null || waves.Length == 0) return;

            if (readyToCountDown)
                countdown -= Time.deltaTime;

            if (countdown <= 0f && !readyToCountDown)
            {
                // start spawning the current wave
                readyToCountDown = false;
                var template = waves[(currentWaveNumber - 1) % waves.Length];
                countdown = template.timeToNextWave;
                StartCoroutine(SpawnWave());
            }

            // If current template's enemiesLeft reaches zero, end the wave and prepare next (infinite allowed)
            var currentTemplate = waves[(currentWaveNumber - 1) % waves.Length];
            if (currentTemplate.enemiesLeft <= 0)
            {
                WaveEnd();
                readyToCountDown = true;
                currentWaveNumber++;
            }
        }

        private IEnumerator SpawnWave()
        {
            var template = waves[(currentWaveNumber - 1) % waves.Length];
            if (template.enemies == null || template.enemies.Length == 0)
            {
                Debug.LogWarning("WaveSpawner: Wave template has no enemy prefabs.");
                yield break;
            }

            int baseCount = template.enemies.Length;
            int spawnCount = baseCount + (currentWaveNumber - 1) * extraEnemiesPerWave;
            template.enemiesLeft = spawnCount; // set how many enemies this wave expects

            int spCount = spawnPoints != null ? spawnPoints.Count : 0;
            if (spCount == 0)
            {
                Debug.LogWarning("WaveSpawner: No spawn points assigned.");
                yield break;
            }

            for (int i = 0; i < spawnCount; i++)
            {
                var spawnPoint = spawnPoints[Random.Range(0, spCount)];
                var enemyPrefab = template.enemies[Random.Range(0, template.enemies.Length)];
                // Instantiate at spawn point world position; set parent for organization
                var enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
                enemy.transform.SetParent(spawnPoint.transform);

                yield return new WaitForSeconds(template.timeToNextEnemy);
            }
        }

        public void WaveEnd()
        {
            Time.timeScale = 0f;
            powerUpManager?.RandomizeNewPowerUps();
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
}
