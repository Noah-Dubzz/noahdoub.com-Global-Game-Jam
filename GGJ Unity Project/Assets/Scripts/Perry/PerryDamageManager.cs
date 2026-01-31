using UnityEngine;

public class PerryDamageManager : MonoBehaviour

{
    private PerryPlayer _perryPlayer = null;
    private float invulTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _perryPlayer = FindAnyObjectByType<PerryPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void MeleeDamage(float damage)
    {
        if (Time.time >= invulTimer)
        {
            _perryPlayer.Health -= damage;
            invulTimer = Time.time + 0.5f;
            
            Debug.Log(_perryPlayer.Health);
        }
        
    }
    
    public void BulletDamage(float damage)
    {
        _perryPlayer.Health -= damage;
        Debug.Log(_perryPlayer.Health);
    }
}
