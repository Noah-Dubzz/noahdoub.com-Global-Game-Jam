using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerryPlayer : MonoBehaviour
{
    public float Health;
    [SerializeField] private float Damage = 5f;
    [SerializeField] public float MaxHealth = 125;
   
    public bool MaskFlipConsentP = false;

    public static PerryPlayer Instance;
    [SerializeField] private MeshRenderer Paura;

    [SerializeField] private Material Supp;
    [SerializeField] private Material Dammage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
        Health = 10;
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MaskManager.Instance.Pdam)
        {
            Paura.material = Dammage;
        }
        if (MaskManager.Instance.Psupp)
        {
            Paura.material = Supp;
        }
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
