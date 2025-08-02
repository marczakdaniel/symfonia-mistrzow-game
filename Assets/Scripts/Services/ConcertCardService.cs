using System.Collections.Generic;
using Assets.Scripts.Data;
using System.Linq;

namespace Services
{
    public class ConcertCardService
    {
        private readonly ConcertCardDeckData allConcertCards;

        public ConcertCardService(ConcertCardDeckData allConcertCards)
        {
            this.allConcertCards = allConcertCards;
        }

        public List<ConcertCardData> GetRandomConcertCards(int count)
        {
            var random = new System.Random();
            var randomCards = allConcertCards.ConcertCards.OrderBy(x => random.Next()).Take(count).ToList();
            return randomCards;
        }
    }
}