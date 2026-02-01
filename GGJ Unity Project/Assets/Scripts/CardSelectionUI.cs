using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectionUI : MonoBehaviour
{
    [SerializeField] private Button cardButton1;
    [SerializeField] private Button cardButton2;
    [SerializeField] private Text cardName1;
    [SerializeField] private Text cardName2;
    [SerializeField] private Text cardDescription1;
    [SerializeField] private Text cardDescription2;

    private Card card1;
    private Card card2;
    private string currentPlayer;

    private void Awake()
    {
        // Ensure buttons call the selection methods even if wired via inspector isn't set up
        if (cardButton1 != null)
        {
            cardButton1.onClick.RemoveAllListeners();
            cardButton1.onClick.AddListener(OnSelectCard1);
            cardButton1.interactable = false;
        }
        if (cardButton2 != null)
        {
            cardButton2.onClick.RemoveAllListeners();
            cardButton2.onClick.AddListener(OnSelectCard2);
            cardButton2.interactable = false;
        }
    }

    public void ShowCardSelection(string playerName, List<Card> cards)
    {
        currentPlayer = playerName;

        if (cards == null || cards.Count < 2)
        {
            Debug.LogError("CardSelectionUI.ShowCardSelection called with invalid card list");
            return;
        }

        // Assign cards
        card1 = cards[0];
        card2 = cards[1];

        // Update UI (guard UI fields)
        if (cardName1 != null) cardName1.text = card1 != null ? card1.cardName : "";
        if (cardDescription1 != null) cardDescription1.text = card1 != null ? card1.description : "";
        if (cardName2 != null) cardName2.text = card2 != null ? card2.cardName : "";
        if (cardDescription2 != null) cardDescription2.text = card2 != null ? card2.description : "";

        // Enable buttons now that cards are assigned
        if (cardButton1 != null) cardButton1.interactable = (card1 != null);
        if (cardButton2 != null) cardButton2.interactable = (card2 != null);

        // Show UI
        gameObject.SetActive(true);
    }

    // Quick test helper you can call from the Inspector during development
    public void TestShow(string playerName)
    {
        if (CardManager.Instance == null)
        {
            Debug.LogError("No CardManager in scene to test ShowCardSelection");
            return;
        }
        var cards = CardManager.Instance.GetRandomCards(playerName);
        ShowCardSelection(playerName, cards);
    }

    public void OnSelectCard1()
    {
        if (CardManager.Instance == null)
        {
            Debug.LogError("CardManager.Instance is null. Ensure a CardManager exists in the scene.");
            return;
        }
        if (card1 == null)
        {
            Debug.LogError("CardSelectionUI: card1 is null when selecting.");
            return;
        }

        CardManager.Instance.EquipCard(currentPlayer, card1);
        gameObject.SetActive(false);
    }

    public void OnSelectCard2()
    {
        if (CardManager.Instance == null)
        {
            Debug.LogError("CardManager.Instance is null. Ensure a CardManager exists in the scene.");
            return;
        }
        if (card2 == null)
        {
            Debug.LogError("CardSelectionUI: card2 is null when selecting.");
            return;
        }

        CardManager.Instance.EquipCard(currentPlayer, card2);
        gameObject.SetActive(false);
    }
}