using UnityEngine;

public class HarleyDamageManager : MonoBehaviour

{
    private HarleyPlayer _harleyPlayer = null;
    private float invulTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _harleyPlayer = FindAnyObjectByType<HarleyPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MeleeDamage(float damage)
    {
        if (Time.time >= invulTimer)
        {
            _harleyPlayer.Health -= damage;
            invulTimer = Time.time + 0.5f;
            
            Debug.Log(_harleyPlayer.Health);
        }
        
    }
    public void BulletDamage(float damage)
    {
        _harleyPlayer.Health -= damage;
        Debug.Log(_harleyPlayer.Health);
    }
}