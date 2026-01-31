using UnityEngine;

[CreateAssetMenu(fileName = "New Power Up", menuName = "Power Up" )]
public class PowerUpsSO : ScriptableObject
{
    public Material powerUpImage;
    public string powerUpTxt;

    public PowerUpEffect effectType;

    public float effectValue;
    public bool isUnique;
}

public enum PowerUpEffect
{
    DamageIncrease,
    MovementSpeedIncrease,
    ProjectileSpeedIncrease,
    AttackSpeedIncrease,
    ReplenishHealth,
    BurstShot
}
