using R3;
using SymfoniaMistrzow.Core.Models;

namespace SymfoniaMistrzow.MVP.Player
{
    public class PlayerModel
    {
        public readonly ReactiveProperty<string> Name = new("");
        public readonly ReactiveDictionary<TokenColor, int> Tokens = new();
        public readonly ReactiveProperty<int> Points = new();
    }
} 