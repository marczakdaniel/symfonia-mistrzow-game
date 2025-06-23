using R3;
using SymfoniaMistrzow.MVP.Card;

namespace SymfoniaMistrzow.MVP.Board
{
    public class BoardModel
    {
        public readonly ReactiveCollection<CardModel> Tier1Cards = new();
        public readonly ReactiveCollection<CardModel> Tier2Cards = new();
        public readonly ReactiveCollection<CardModel> Tier3Cards =new();

        // Add nobles, tokens, etc.
    }
} 