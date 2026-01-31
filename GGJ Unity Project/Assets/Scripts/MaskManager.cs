using System.Runtime.CompilerServices;
using UnityEngine;

public class MaskManager : MonoBehaviour
{
    [SerializeField]public HarleyPlayer harleyPlayer;
    [SerializeField]public PerryPlayer perryPlayer;
    public static MaskManager Instance;

    private bool Hsupp = true;
    private bool HDam = false;
    private bool Psupp = false;
    private bool Pdam = true;
    private bool canflip = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        MaskManager.Instance = this;
        HarleySupport();
        PerryDamage();
    }

    // Update is called once per frame
    void Update()
    {
        harleyPlayer = FindAnyObjectByType<HarleyPlayer>();
        perryPlayer = FindAnyObjectByType<PerryPlayer>();
        MaskFlipper();
    }

    public void MaskFlipper()
    {
        
        if(harleyPlayer.MaskFlipConsentH  == true && perryPlayer.MaskFlipConsentP == true)
        {
            Debug.Log("both are true");
            canflip = true;

            if( Hsupp && Pdam && canflip)
            {
                Hsupp = false;
                Pdam = false;
                HDam = true;
                Psupp = true;
                HarleyDamage();
                PerrySupport();
                canflip = false;
               
            }

            if (HDam && Psupp && canflip)
            {
                Hsupp = true;
                Pdam = true;
                HDam = false;
                Psupp = false;
                HarleySupport();
                PerryDamage();
                canflip = false;
            }
            harleyPlayer.MaskFlipConsentH = false;
            perryPlayer.MaskFlipConsentP = false;
        }
    }

   

    public void HarleySupport()
    {
        Debug.Log("Harley is in support Mode");
    }

    public void HarleyDamage()
    {
        Debug.Log("Harley is in Damage Mode");
    }

    public void PerrySupport()
    {
        Debug.Log("perry is in support Mode");
    }
    public void PerryDamage()
    {
        Debug.Log("Perry is in damage mode");
    }

    
}
