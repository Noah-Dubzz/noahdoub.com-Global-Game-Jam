using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerryPlayer : MonoBehaviour
{
    [SerializeField] private float Health = 125;
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
