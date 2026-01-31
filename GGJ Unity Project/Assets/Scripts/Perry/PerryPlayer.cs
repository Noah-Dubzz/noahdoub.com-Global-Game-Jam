using UnityEngine;
using UnityEngine.InputSystem;

public class PerryPlayer : MonoBehaviour
{
    [SerializeField] private float Health = 125;
    [SerializeField] private float Damage = 5f;

    public bool MaskFlipConsentP = false;
    HarleyPlayer HarleyPlayer;

    private bool supportM = false;
    private bool DamageM = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        DamageMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (MaskFlipConsentP == true && HarleyPlayer.MaskFlipConsentH == true)
        {
            FlipMask();
            MaskFlipConsentP = false;
        }
    }

    public void MaskFlip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MaskFlipConsentP = true;

            
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
        Debug.Log($"<collor=green> Perry is in support Mode</color>");

    }

    private void DamageMode()
    {
        Debug.Log($"<collor=blue> perry is in Damage Mode</color>");
    }
}
