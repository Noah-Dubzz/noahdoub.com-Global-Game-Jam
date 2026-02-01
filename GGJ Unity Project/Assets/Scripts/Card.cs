using UnityEngine;

namespace Game.Cards
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "Card System/Card")]
    public class Card : ScriptableObject
    {
        public string cardName;
        public string description;
        public int level;
        public Sprite cardImage; // Optional: Add a sprite for the card's visual representation

        // Add any specific effects or stats for the card
        public int damageBoost;
        public float rangeBoost;

        // Method to upgrade the card
        public void UpgradeCard()
        {
            level++;
            damageBoost += 10; // Example: Increase damage boost by 10 per level
            rangeBoost += 0.5f; // Example: Increase range boost by 0.5 per level
        }
    }
}