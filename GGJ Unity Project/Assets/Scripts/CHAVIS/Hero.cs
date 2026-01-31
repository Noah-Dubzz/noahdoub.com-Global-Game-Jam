using UnityEngine;

namespace CHAVIS
{
    public class Hero : MonoBehaviour
    {
        public static Hero Instance { get; private set; }

        public float damage = 1f;
        public float health = 10f;

        private void Awake()
        {
            Instance = this;
        }

        public void ApplyPU(PowerUpsSO pu)
        {
            // minimal placeholder: developer should implement proper PU handling
            if (pu == null) return;
        }

        public void TakeDamage(float amount)
        {
            health -= amount;
            if (health <= 0f)
            {
                // placeholder death handling
                Destroy(gameObject);
            }
        }
    }
}
