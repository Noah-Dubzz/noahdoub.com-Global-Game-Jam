using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerryPlayer : MonoBehaviour
{
    [SerializeField] public float Health = 125;
    [SerializeField] private float Damage = 5f;

   
    public bool MaskFlipConsentP = false;
    
    public static PerryPlayer Instance;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Health <= 0)
        {
            Debug.Log("PERRY DOWN");
        }*/
    }

    public void MaskFlip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MaskFlipConsentP = true;
            Debug.Log("Perry has consented for a mask switch");

        }
    }
}
