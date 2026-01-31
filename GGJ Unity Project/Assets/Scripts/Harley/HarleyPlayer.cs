using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class HarleyPlayer : MonoBehaviour
{
    [SerializeField] private float Health = 100f;
    [SerializeField] private float Damage = 5f;
    
    
    

    public bool MaskFlipConsentH = false;
    private bool supportM = true;
    private bool DamageM = false;
    [SerializeField] public GameObject PerryPlayer;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // PerryPlayer = PerryPlayer.GetComponent<PerryPlayer>();
        SupportMode();
    }

    // Update is called once per frame
    void Update()
    {
        //if (MaskFlipConsentH && perryPlayer.MaskFlipConsentP == true)
        {
            FlipMask();
            MaskFlipConsentH = false;
        }
    }

    public void MaskFlip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MaskFlipConsentH = true;

            
        }
    }

    private void FlipMask()
    {
        if (supportM)
        {
            supportM = false;

            DamageMode();
            DamageM = true;
        }

        if (DamageM)
        {
            supportM = true;
            SupportMode();
            DamageM = false;
        }
    }
    private void SupportMode()
    {
        //Debug.Log($"<collor=pink> Harley is in support Mode</color>");

    }

    private void DamageMode()
    {
        //Debug.Log($"<collor=purple> Harley is in Damage Mode</color>");
    }
}
