using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerryDamageManager : MonoBehaviour

{

    public float meleeInvulnerabilityTime = 1f;
    private float _invulTimer = 0f;
    private bool _isInvul = false;

    private Renderer _renderer;
    private Material _perryMat;

    private Color _basicColor;
    
    void Start()
    {
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
        if (PerryPlayer.Instance.Health <= 0)
        {
            SceneManager.LoadScene("rmLose");
        }
    }
    
    public void MeleeDamage(float damage)
    {
        if (Time.time >= _invulTimer && !_isInvul)
        {
            if (PerryPlayer.Instance.PerryShield > 0 && PerryPlayer.Instance.Paura.enabled)
            {
                PerryPlayer.Instance.PerryShield -= damage;
                if (PerryPlayer.Instance.PerryShield <= 0)
                {
                    PerryPlayer.Instance.Health -= (damage - Math.Abs(PerryPlayer.Instance.PerryShield));
                    PerryPlayer.Instance.PerryShield = 0;
                    PerryPlayer.Instance.Paura.enabled = false;
                }
            }
            if (PerryPlayer.Instance.Paura.enabled == false)
            {
                PerryPlayer.Instance.Health -= damage;
                _invulTimer = Time.time + meleeInvulnerabilityTime;
                _perryMat.color = Color.red;
                _isInvul = true;
            }
        }
        HealthBarController.Instance.UpdateHealthBar(1);
    }
    
    public void BulletDamage(float damage)
    {
        if(PerryPlayer.Instance.PerryShield > 0 && PerryPlayer.Instance.Paura.enabled)
        {
            PerryPlayer.Instance.PerryShield -= damage;
            if (PerryPlayer.Instance.PerryShield <= 0)
            {
                PerryPlayer.Instance.Health -= (damage - Math.Abs(PerryPlayer.Instance.PerryShield));
                PerryPlayer.Instance.PerryShield = 0;
                PerryPlayer.Instance.Paura.enabled = false;
            }
        }
        else
        {
            PerryPlayer.Instance.Health -= damage;
        }
        
        HealthBarController.Instance.UpdateHealthBar(1);
    }
}
