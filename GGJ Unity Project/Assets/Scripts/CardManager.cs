using System.Collections.Generic;
using UnityEngine;
using Game.Cards;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Card> harleyCards; // List of all cards for Harley
    public List<Card> perryCards; // List of all cards for Perry

    private List<Card> harleyEquippedCards = new List<Card>(); // Harley's equipped cards
    private List<Card> perryEquippedCards = new List<Card>(); // Perry's equipped cards

    // Method to get 2 random cards for a player
    public List<Card> GetRandomCards(string playerName)
    {
        List<Card> availableCards = playerName == "Harley" ? harleyCards : perryCards;
        List<Card> randomCards = new List<Card>();

        // Shuffle the available cards and pick 2
        for (int i = 0; i < 2; i++)
        {
            if (availableCards.Count > 0)
            {
                int randomIndex = Random.Range(0, availableCards.Count);
                randomCards.Add(availableCards[randomIndex]);
                availableCards.RemoveAt(randomIndex); // Remove the card to avoid duplicates
            }
        }

        return randomCards;
    }

    // Method to equip a card for a player
    public void EquipCard(string playerName, Card card)
    {
        if (playerName == "Harley")
        {
            EquipCardForPlayer(harleyEquippedCards, HarleyPlayer.Instance, card);
        }
        else if (playerName == "Perry")
        {
            EquipCardForPlayer(perryEquippedCards, PerryPlayer.Instance, card);
        }
    }

    // Helper method to equip a card for a specific player
    private void EquipCardForPlayer(List<Card> equippedCards, MonoBehaviour player, Card card)
    {
        if (equippedCards.Count >= 2)
        {
            // If the player already has 2 cards equipped, replace the first card
            equippedCards.RemoveAt(0);
        }

        equippedCards.Add(card);

        // Apply the card's effects to the player
        if (player is HarleyPlayer harley)
        {
            harley.AddCard(card);
        }
        else if (player is PerryPlayer perry)
        {
            perry.AddCard(card);
        }

        Debug.Log($"{card.cardName} equipped for {player.name}");
    }
}