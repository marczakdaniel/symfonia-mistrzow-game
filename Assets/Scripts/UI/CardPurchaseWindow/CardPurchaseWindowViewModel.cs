using System.Collections.Generic;
using DefaultNamespace.Data;
using R3;

namespace UI.CardPurchaseWindow
{
    public class CardPurchaseWindowViewModel
    {
        public MusicCardData MusicCardData { get; private set; }
        public bool IsFromMusicCardDetailsPanel { get; private set; }

        public CardPurchaseWindowViewModel()
        {
        }

        public void SetMusicCardData(MusicCardData musicCardData)
        {
            MusicCardData = musicCardData;
        }

        public void SetIsFromMusicCardDetailsPanel(bool isFromMusicCardDetailsPanel)
        {
            IsFromMusicCardDetailsPanel = isFromMusicCardDetailsPanel;
        }

    }
}