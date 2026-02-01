using CHAVIS;
using Unity.VisualScripting;
using UnityEngine;

namespace Micah
{
    public class MeleeDamageScript : MonoBehaviour
    {
        public float damage = 10f;
        
        private HarleyDamageManager _harleyDamageManager;
        private PerryDamageManager _perryDamageManager;
        
        private int _perryLayer;
        private int _harleyLayer;

        void Awake()
        {
            _perryLayer = LayerMask.NameToLayer("Perry");
            _harleyLayer = LayerMask.NameToLayer("Harley");
            
            _perryDamageManager = FindAnyObjectByType<PerryDamageManager>();
            _harleyDamageManager = FindAnyObjectByType<HarleyDamageManager>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
        
            int otherLayer = other.gameObject.layer;
        
            if (otherLayer == _harleyLayer)
            {
                _harleyDamageManager = FindAnyObjectByType<HarleyDamageManager>();
                _harleyDamageManager?.MeleeDamage(damage + InGameMenus.Instance.waveNumber);
            }
            else if (otherLayer == _perryLayer)
            {
                _perryDamageManager = FindAnyObjectByType<PerryDamageManager>();
                _perryDamageManager?.MeleeDamage(damage + InGameMenus.Instance.waveNumber);
            }
        }
    }
}