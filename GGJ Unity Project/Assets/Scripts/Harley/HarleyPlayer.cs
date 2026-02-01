using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class HarleyPlayer : MonoBehaviour
{
    [SerializeField] public float Health;
    public float shieldHealth;
    [SerializeField] public float MaxHealth = 100f;
    [SerializeField] public float Damage = 5f;
    [SerializeField] public float DashDamage = 3f;
    [SerializeField] public float HealthRegen = 5f;
    [SerializeField] public float HarleyShield = 0f;
    public bool canDamageH = false;

    [SerializeField] public SpriteRenderer HarleySprite;


    public static HarleyPlayer Instance;
    [SerializeField] public MeshRenderer Haura;


    [SerializeField] private Material Supp;
    [SerializeField] private Material Dammage;


    public bool MaskFlipConsentH = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
        Health = MaxHealth;
    }
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        if (HarleyShield > 0)
        {
            Haura.enabled = true;

            
        }
        

        if (MaskManager.Instance.HDam)
        {
            Haura.material = Dammage;

        }
        if (MaskManager.Instance.Hsupp)
        {
            Haura.material = Supp;

        }
    }

    public void MaskFlip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MaskFlipConsentH = true;
            Debug.Log("Harley has consented for a mask switch");
            if (MaskManager.Instance.HDam)
            { 
            canDamageH = true;
            }
            if(MaskManager.Instance.Hsupp)
            {
                canDamageH = false;
            }       
        }
    }    
}
