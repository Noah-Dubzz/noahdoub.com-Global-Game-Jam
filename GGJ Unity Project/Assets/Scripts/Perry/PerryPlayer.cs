using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Cards;

public class PerryPlayer : MonoBehaviour
{
    public float Health;
    public float shieldHealth;
    [SerializeField] public float Damage = 5f;
    [SerializeField] public float MaxHealth = 125;
    [SerializeField] public float PerryShield = 0f;
    [SerializeField] public float ShieldProtects = 20f;
    [SerializeField] public SpriteRenderer PerrySprite;
    public bool canDamageP = true;

    public bool MaskFlipConsentP = false;

    public static PerryPlayer Instance;
    [SerializeField] public MeshRenderer Paura;

    [SerializeField] private Material Supp;
    [SerializeField] private Material Dammage;

    public List<Card> ownedCards = new List<Card>();

    // Add a card to Perry and apply its effects
    public void AddCard(Card card)
    {
        if (card == null) return;
        if (!ownedCards.Contains(card))
        {
            ownedCards.Add(card);
            ApplyCardEffect(card);
            Debug.Log($"Card {card.cardName} added to Perry.");
        }
    }

    // Remove the effects of a card
    public void RemoveCardEffect(Card card)
    {
        if (card == null) return;
        Health -= card.damageBoost;
        MaxHealth -= Mathf.Round(card.rangeBoost);
        Debug.Log($"Removed card effect: {card.cardName}");
    }

    private void ApplyCardEffect(Card card)
    {
        if (card == null) return;
        Health += card.damageBoost;
        MaxHealth += Mathf.Round(card.rangeBoost);
        Debug.Log($"Applied card effect: {card.cardName}");
    }


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


        if (PerryShield > 0)
        {
            Paura.enabled = true;


        }
        

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
            if (MaskManager.Instance.HDam)
            {
                canDamageP = true;
            }
            if (MaskManager.Instance.Hsupp)
            {
                canDamageP = false;
            }

        }
    }
}
