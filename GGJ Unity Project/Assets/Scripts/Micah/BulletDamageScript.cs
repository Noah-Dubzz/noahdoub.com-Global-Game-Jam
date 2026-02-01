using System;
using Micah;
using Unity.VisualScripting;
using UnityEngine;

public class BulletDamageScript : MonoBehaviour
{
    private PerryDamageManager _perryDamageManager;
    private HarleyDamageManager _harleyDamageManager;
    private float _damage;
    
    private int _perryLayer;
    private int _harleyLayer;
    

    void Awake()
    {
        _perryLayer = LayerMask.NameToLayer("Perry");
        _harleyLayer = LayerMask.NameToLayer("Harley");
    }

    public void Setup(PerryDamageManager perryManager, HarleyDamageManager harleyManager, float damage)
    {
        _perryDamageManager = perryManager;
        _harleyDamageManager = harleyManager;
        _damage = damage;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Player"))
        {
            return;
        }
        
        int otherLayer = other.gameObject.layer;
        
        if (otherLayer == _harleyLayer)
        {
            _harleyDamageManager?.BulletDamage(_damage);
        }
        else if (otherLayer == _perryLayer)
        {
            _perryDamageManager?.BulletDamage(_damage);
        }

        gameObject.SetActive(false);
        // Destroy(gameObject);
    }
}
