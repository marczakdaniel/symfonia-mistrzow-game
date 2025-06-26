using System;
using R3;

namespace DefaultNamespace.Models
{
    public class PlayerModel : IDisposable
    {
        private readonly string playerId;
        private readonly string playerName;
        private readonly ReactiveProperty<int> score;
        
        

        public void Dispose()
        {
            
        }
    }
}