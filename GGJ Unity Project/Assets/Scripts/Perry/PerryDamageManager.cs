using TMPro.EditorUtilities;
using UnityEngine;

public class PerryDamageManager : MonoBehaviour

{

    public float meleeInvulnerabilityTime = 1f;
    private PerryPlayer _perryPlayer = null;
    private float _invulTimer = 0f;

    private Renderer _renderer;
    private Material _perryMat;

    private Color _basicColor;
    
    void Start()
    {
        _perryPlayer = FindAnyObjectByType<PerryPlayer>();
        _renderer = GetComponentInChildren<Renderer>();
        _perryMat = _renderer.material;
        _basicColor = _perryMat.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (_invulTimer <= Time.time)
        {
            _perryMat.color = _basicColor;
                
        }
    }
    
    public void MeleeDamage(float damage)
    {
        
        
        if (Time.time >= _invulTimer)
        {
            _perryPlayer.Health -= damage;
            _invulTimer = Time.time + meleeInvulnerabilityTime;
            _perryMat.color = Color.red;
            
            Debug.Log(_perryPlayer.Health);
        }
        
    }
    
    public void BulletDamage(float damage)
    {
        _perryPlayer.Health -= damage;
        //Debug.Log(_perryPlayer.Health);
    }
}
