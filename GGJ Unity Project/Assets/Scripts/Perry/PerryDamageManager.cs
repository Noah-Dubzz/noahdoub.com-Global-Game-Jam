using TMPro.EditorUtilities;
using UnityEngine;

public class PerryDamageManager : MonoBehaviour

{

    public float meleeInvulnerabilityTime = 1f;
    private PerryPlayer _perryPlayer = null;
    private float _invulTimer = 0f;
    private bool _isInvul = false;

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
        if (_isInvul)
        {
            if (_invulTimer <= Time.time)
            {
                _renderer.enabled = true;
                _isInvul = false;
                _perryMat.color = _basicColor;
            }
            else if(Time.time -(_invulTimer - meleeInvulnerabilityTime) > 0.25f)
            {
                _perryMat.color = _basicColor;
                _renderer.enabled = (Time.time % 0.5f) < 0.4f;
            }
        }
    }
    
    public void MeleeDamage(float damage)
    {
        if (Time.time >= _invulTimer && !_isInvul)
        {
            _perryPlayer.Health -= damage;
            _invulTimer = Time.time + meleeInvulnerabilityTime;
            _perryMat.color = Color.red;
            _isInvul = true;
            
            Debug.Log(_perryPlayer.Health);
        }
        
    }
    
    public void BulletDamage(float damage)
    {
        _perryPlayer.Health -= damage;
        //Debug.Log(_perryPlayer.Health);
    }
}
