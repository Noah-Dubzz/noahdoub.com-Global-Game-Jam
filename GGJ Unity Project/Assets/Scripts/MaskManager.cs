using System.Runtime.CompilerServices;
using UnityEngine;

public class MaskManager : MonoBehaviour
{
    [SerializeField]public HarleyPlayer harleyPlayer;
    [SerializeField]public PerryPlayer perryPlayer;
    public static MaskManager Instance;
    public Sprite Harleysupp;
    public Sprite Perrysupp;
    public Sprite HarleyDam;
    public Sprite PerryDam;
    
    public bool Hsupp = true;
    public bool HDam = false;
    public bool Psupp = false;
    public bool Pdam = true;
    public bool canflip = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
       Instance = this;
        harleyPlayer = FindAnyObjectByType<HarleyPlayer>();
        perryPlayer = FindAnyObjectByType<PerryPlayer>();

        if (harleyPlayer == null && perryPlayer == null)
        {
            Debug.LogWarning("No players found in the scene. The game will run without players.");
        }
        else if (harleyPlayer == null)
        {
            Debug.LogWarning("HarleyPlayer is not found in the scene. Only PerryPlayer is available.");
        }
        else if (perryPlayer == null)
        {
            Debug.LogWarning("PerryPlayer is not found in the scene. Only HarleyPlayer is available.");
        }
    }
    void Start()
    {
       
        
        HarleySupport();
        PerryDamage();
    }

    // Update is called once per frame
    void Update()
    {
        if (harleyPlayer == null || perryPlayer == null)
        {
            // No players in the scene, skip Update logic
            harleyPlayer = FindAnyObjectByType<HarleyPlayer>();
            perryPlayer = FindAnyObjectByType<PerryPlayer>();
            return;
        }

        MaskFlipper();
    }

    public void MaskFlipper()
    {
        if (harleyPlayer != null && perryPlayer != null)
        {
            if (harleyPlayer.MaskFlipConsentH && perryPlayer.MaskFlipConsentP)
            {
                Debug.Log("both are true");
                canflip = true;

                if (Hsupp && Pdam && canflip)
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
        else if (harleyPlayer != null)
        {
            Debug.Log("Only HarleyPlayer is present. Skipping PerryPlayer logic.");
            // Handle HarleyPlayer-specific logic if needed
        }
        else if (perryPlayer != null)
        {
            Debug.Log("Only PerryPlayer is present. Skipping HarleyPlayer logic.");
            // Handle PerryPlayer-specific logic if needed
        }
    }

   

    public void HarleySupport()
    {
        if (HarleyPlayer.Instance == null || HarleyPlayer.Instance.HarleySprite == null)
        {
            Debug.LogError("HarleyPlayer instance or HarleySprite is not assigned.");
            return;
        }

        Debug.Log("Harley is in support Mode");
        HarleyPlayer.Instance.HarleySprite.sprite = Harleysupp;

    }

    public void HarleyDamage()
    {
        if (HarleyPlayer.Instance == null || HarleyPlayer.Instance.HarleySprite == null)
        {
            Debug.LogError("HarleyPlayer instance or HarleySprite is not assigned.");
            return;
        }

        Debug.Log("Harley is in Damage Mode");
        HarleyPlayer.Instance.HarleySprite.sprite = HarleyDam;

    }

    public void PerrySupport()
    {
        if (PerryPlayer.Instance == null || PerryPlayer.Instance.PerrySprite == null)
        {
            Debug.LogError("PerryPlayer instance or PerrySprite is not assigned.");
            return;
        }

        Debug.Log("Perry is in support Mode");
        PerryPlayer.Instance.PerrySprite.sprite = Perrysupp;
    }
    public void PerryDamage()
    {
        if (PerryPlayer.Instance == null || PerryPlayer.Instance.PerrySprite == null)
        {
            Debug.LogError("PerryPlayer instance or PerrySprite is not assigned.");
            return;
        }

        Debug.Log("Perry is in damage mode");
         PerryPlayer.Instance.PerrySprite.sprite = PerryDam;

    }

    
}
