using System.Collections.Generic;
using R3;

namespace SymfoniaMistrzow.Core.Models
{
    public class Player
    {
        public readonly ReactiveProperty<string> PlayerName = new("");
        public readonly ReactiveDictionary<TokenColor, int> Tokens = new();
        public readonly ReactiveCollection<Card> OwnedCards = new();

        // Add other player-related properties like points, reserved cards etc.
    }
} 