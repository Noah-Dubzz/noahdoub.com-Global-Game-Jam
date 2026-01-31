using System.Collections.Generic;
using UnityEngine;

namespace CHAVIS
{
    public class PowerUpManager : MonoBehaviour
    {
        [SerializeField] private GameObject puSelectionUI;
        [SerializeField] private GameObject puPrefabs;
        [SerializeField] private Transform puPositionOne;
        [SerializeField] private Transform puPositionTwo;
        [SerializeField] private List<PowerUpsSO> PowerUpList;
        public static PowerUpManager Instance { get; private set; }

        private GameObject puOne, puTwo;

        private readonly List<PowerUpsSO> alreadySelectedPU = new();

        private void Awake()
        {
            Instance = this;

            if (InGameMenus.Instance != null )
            {
                InGameMenus.Instance.OnStateChanged += HandleGameStateChanged;
            }
        }

        private void OnDisable()
        {
            if (InGameMenus.Instance != null)
            {
                InGameMenus.Instance.OnStateChanged -= HandleGameStateChanged;
            }
        }

        private void HandleGameStateChanged(InGameMenus.GameState state)
        {
            if (state == InGameMenus.GameState.PowerUpSelection)
            {
                RandomizeNewPowerUps();
            }
        }

        public void RandomizeNewPowerUps()
        {
            if (puOne != null) Destroy(puOne);
            if (puTwo != null) Destroy(puTwo);

            List<PowerUpsSO> selectedPUs = new();

            List<PowerUpsSO> availiablePUs = new(PowerUpList);

            availiablePUs.RemoveAll(powerUp => powerUp.isUnique && alreadySelectedPU.Contains(powerUp));
            
            if (availiablePUs.Count < 2)
            {
                //Debug.Log("Not enough Power Ups");
                return;
            }

            while (selectedPUs.Count < 2)
            {
                PowerUpsSO randomPU = availiablePUs[Random.Range(0, availiablePUs.Count)];
                if (!selectedPUs.Contains(randomPU))
                {
                    selectedPUs.Add(randomPU);
                }
            }

            puOne = InstantiatePU(selectedPUs[0], puPositionOne);
            puTwo = InstantiatePU(selectedPUs[1], puPositionTwo);
        }

        private GameObject InstantiatePU(PowerUpsSO puSO, Transform position)
        {
            GameObject powerUpGO = Instantiate(puPrefabs, position.position, Quaternion.identity, position);
            PowerUps powerUp = powerUpGO.GetComponent<PowerUps>();
            powerUp.SetUp(puSO);
            return powerUpGO;
        }

        public void SelectPowerUp(PowerUpsSO selectedPU)
        {
            if (!alreadySelectedPU.Contains(selectedPU))
            {
                alreadySelectedPU.Add(selectedPU);
            }

            InGameMenus.Instance.ChangeState(InGameMenus.GameState.Playing);
        }

        public void ShowPUSelection()
        {
            puSelectionUI.SetActive(true);
        }
        public void HidePUSelection()
        {
            puSelectionUI.SetActive(false);
        }
    }
}
