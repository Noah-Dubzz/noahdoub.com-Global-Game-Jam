using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class HarleyPlayer : MonoBehaviour
{
    [SerializeField] private float Health = 100f;
    [SerializeField] private float Damage = 5f;
    public static HarleyPlayer Instance;

  

    public bool MaskFlipConsentH = false;

    public float CurrentHealth { get; private set; }
    public float AttackDamage => Damage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
        CurrentHealth = Health;
        
    }
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MaskFlip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MaskFlipConsentH = true;
            Debug.Log("Harley has consented for a mask switch");

            
        }
    }

    public void ApplyDamage(float amount)
    {
        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
    }

   
}
