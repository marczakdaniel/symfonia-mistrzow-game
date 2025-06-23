using System.Collections.Generic;

namespace SymfoniaMistrzow.Core.Models
{
    public enum TokenColor
    {
        Blue,
        Green,
        Red,
        Black,
        White,
        Gold // Joker
    }

    public class Card
    {
        public int Id;
        public int Level; // e.g., 1, 2, 3
        public int Points;
        public TokenColor GemColor; // The gem this card provides
        public Dictionary<TokenColor, int> Cost;
    }
} 