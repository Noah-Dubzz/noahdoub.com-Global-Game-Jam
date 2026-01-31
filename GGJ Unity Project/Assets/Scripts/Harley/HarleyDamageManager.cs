using UnityEngine;

public class HarleyDamageManager : MonoBehaviour

{
    public float meleeInvulnerabilityTime = 1f;
    private HarleyPlayer _harleyPlayer = null;
    private float _invulTimer = 0f;
    
    private Renderer _renderer;
    private Material _harleyMat;

    private Color _basicColor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _harleyPlayer = FindAnyObjectByType<HarleyPlayer>();
        _renderer = GetComponentInChildren<Renderer>();
        _harleyMat = _renderer.material;
        _basicColor = _harleyMat.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (_invulTimer <= Time.time)
        {
            _harleyMat.color = _basicColor;
                
        }
    }

    public void MeleeDamage(float damage)
    {
        if (Time.time >= _invulTimer)
        {
            _harleyPlayer.Health -= damage;
            _invulTimer = Time.time + meleeInvulnerabilityTime;
            _harleyMat.color = Color.red;
            
            Debug.Log(_harleyPlayer.Health);
        }
        
    }
    public void BulletDamage(float damage)
    {
        _harleyPlayer.Health -= damage;
        //Debug.Log(_harleyPlayer.Health);
    }
}