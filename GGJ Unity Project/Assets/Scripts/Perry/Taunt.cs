using UnityEngine;

namespace Perry
{
    public class Taunt : MonoBehaviour
    {
        public void ActivateTaunt()
        {
            if (OBPSpawning.Instance != null)
            {
                foreach (var enemyPoolObj in OBPSpawning.Instance.ActiveEnemies)
                {
                    if (enemyPoolObj.TryGetComponent<Enemy1>(out var e1))
                    {
                        e1.FindTargetPlayer(true);
                    }
                    else if (enemyPoolObj.TryGetComponent<Enemy_Projectile>(out var ep))
                    {
                        ep.FindTargetPlayer(true);
                    }
                }
            }
        }
    }
}