using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class HarleyPlayer : MonoBehaviour
{
    [SerializeField] public float Health = 100f;
    [SerializeField] private float Damage = 5f;
    public static HarleyPlayer Instance;

  

    public bool MaskFlipConsentH = false;
    
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
        if (Health <= 0)
        {
            Debug.Log("HARLEY DOWN");
        }
    }

    public void MaskFlip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MaskFlipConsentH = true;
            Debug.Log("Harley has consented for a mask switch");

            
        }
    }

   
}
