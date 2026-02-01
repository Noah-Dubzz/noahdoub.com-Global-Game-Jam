using UnityEngine;

namespace CHAVIS
{
    [CreateAssetMenu(fileName = "New Power Up", menuName = "Power Up" )]
    public class PowerUpsSO : ScriptableObject
    {
        public Sprite image;
        public string description;
        public PowerUpEffect powerUpEffect;
        public float effectValue1;    
        public float effectValue2;
        public bool isUnique;
    }

    public enum PowerUpEffect
    {
        DashDamageIncrease,
        TauntDamageIncrease,
        HealthRegenIncrease,
        ShieldHealthIncrease,
        ShieldKBRIncrease
    }
}
