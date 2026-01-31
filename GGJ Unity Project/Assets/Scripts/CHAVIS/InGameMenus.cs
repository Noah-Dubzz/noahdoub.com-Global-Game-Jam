using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CHAVIS
{
    public class InGameMenus : MonoBehaviour
    {
        public static InGameMenus Instance;
        public GameObject pauseMenu;
        GameState currentState;
        public static bool isPaused = false;
        public event Action<GameState> OnStateChanged;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            pauseMenu.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        public void PauseGame()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
        public void ResumeGame()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        public void QuitGame()
        {
            Application.Quit();
        }

        public void ChangeState(GameState newState)
        {
            currentState = newState;
            OnStateChanged?.Invoke(newState);
            HandleChangedState();
        }
        private void HandleChangedState()
        {
            switch (currentState)
            {
                case GameState.Playing:
                    PowerUpManager.Instance.HidePUSelection();
                    break;
                case GameState.PowerUpSelection:
                    PowerUpManager.Instance.ShowPUSelection();
                    break;
            }
        }
        public enum GameState
        {
            Playing,
            PowerUpSelection
        }
    }
}
