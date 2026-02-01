using UnityEngine;

public class HarleyDamageManager : MonoBehaviour

{
    public float meleeInvulnerabilityTime = 1f;
    private HarleyPlayer _harleyPlayer = null;
    private float _invulTimer = 0f;
    private bool _isInvul = false;
    
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
        if (_isInvul)
        {
            if (_invulTimer <= Time.time)
            {
                _renderer.enabled = true;
                _isInvul = false;
                _harleyMat.color = _basicColor;
            }
            else if(Time.time -(_invulTimer - meleeInvulnerabilityTime) > 0.25f)
            {
                _harleyMat.color = _basicColor;
                _renderer.enabled = (Time.time % 0.5f) < 0.4f;
            }
        }
    }

    public void MeleeDamage(float damage)
    {
        if (Time.time >= _invulTimer && !_isInvul)
        {
            if (HarleyPlayer.Instance.HarleyShield > 0 && HarleyPlayer.Instance.Haura.enabled)
            {
                HarleyPlayer.Instance.HarleyShield -= damage;
            }
            if (HarleyPlayer.Instance.HarleyShield < 0 && HarleyPlayer.Instance.Haura.enabled)
            {
                _harleyPlayer.Health -= HarleyPlayer.Instance.HarleyShield;
                HarleyPlayer.Instance.HarleyShield = 0;
                HarleyPlayer.Instance.Haura.enabled = false;
            }
            if (HarleyPlayer.Instance.Haura.enabled == false)
            {
                _harleyPlayer.Health -= damage;
                _invulTimer = Time.time + meleeInvulnerabilityTime;
                _harleyMat.color = Color.red;
                _isInvul = true;

                Debug.Log(_harleyPlayer.Health);
            }
        }
        
    }
    public void BulletDamage(float damage)
    {
        if(HarleyPlayer.Instance.HarleyShield > 0 && HarleyPlayer.Instance.Haura.enabled)
        {
            HarleyPlayer.Instance.HarleyShield -= damage;
        }
        if (HarleyPlayer.Instance.HarleyShield < 0 &&  HarleyPlayer.Instance.Haura.enabled)
        {
            _harleyPlayer.Health -= HarleyPlayer.Instance.HarleyShield;
            HarleyPlayer.Instance.HarleyShield = 0;
            HarleyPlayer.Instance.Haura.enabled = false;
        }
        if (HarleyPlayer.Instance.Haura.enabled == false)
        {
            _harleyPlayer.Health -= damage;
            //Debug.Log(_harleyPlayer.Health);
        }
    }
}