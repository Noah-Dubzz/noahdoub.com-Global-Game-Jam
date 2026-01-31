using UnityEngine;

namespace CHAVIS
{
    public class InGameMenus : MonoBehaviour
    {
        public static InGameMenus Instance { get; private set; }

        public enum GameState
        {
            Playing,
            PowerUpSelection,
            Paused
        }

        public delegate void StateChanged(GameState state);
        public event StateChanged OnStateChanged;

        private void Awake()
        {
            Instance = this;
        }

        public void ChangeState(GameState state)
        {
            OnStateChanged?.Invoke(state);
        }
    }
}
