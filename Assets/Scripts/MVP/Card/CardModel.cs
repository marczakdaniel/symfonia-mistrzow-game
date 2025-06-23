using R3;
using SymfoniaMistrzow.Core.Models;

namespace SymfoniaMistrzow.MVP.Card
{
    public class CardModel
    {
        public readonly ReactiveProperty<int> Points = new();
        public readonly ReactiveProperty<TokenColor> GemColor = new();
        public readonly ReactiveDictionary<TokenColor, int> Cost = new();
        // Add other properties like card artwork, level, etc.
    }
} 