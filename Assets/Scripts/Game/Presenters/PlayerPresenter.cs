using R3;
using SplendorGame.Core.MVP;
using SplendorGame.Game.Models;
using SplendorGame.Game.Views;
using System.Linq;

namespace SplendorGame.Game.Presenters
{
    /// <summary>
    /// Presenter for individual player, connecting PlayerModel to PlayerView
    /// </summary>
    public class PlayerPresenter : BasePresenter<PlayerModel, PlayerView>
    {
        public PlayerPresenter(PlayerModel model, PlayerView view) : base(model, view)
        {
        }
        
        protected override void BindViewToModel()
        {
            // Bind player name changes
            Model.PlayerName
                .Subscribe(name => View.SetPlayerName(name))
                .AddTo(_disposables);
            
            // Bind score changes
            Model.Score
                .Subscribe(score => View.SetScore(score))
                .AddTo(_disposables);
            
            // Bind resource changes
            Model.Resources
                .Subscribe(resources => View.UpdateResources(resources))
                .AddTo(_disposables);
            
            // Bind gold token changes
            Model.GoldTokens
                .Subscribe(goldTokens => View.UpdateGoldTokens(goldTokens))
                .AddTo(_disposables);
            
            // Bind bonus changes
            Model.Bonuses
                .Subscribe(bonuses => View.UpdateBonuses(bonuses))
                .AddTo(_disposables);
            
            // Bind purchased cards changes
            Model.PurchasedCards.ObserveCountChanged()
                .Subscribe(_ => View.UpdatePurchasedCards(Model.PurchasedCards.ToList()))
                .AddTo(_disposables);
            
            // Bind reserved cards changes
            Model.ReservedCards.ObserveCountChanged()
                .Subscribe(_ => View.UpdateReservedCards(Model.ReservedCards.ToList()))
                .AddTo(_disposables);
        }
        
        public void SetCurrentPlayer(bool isCurrentPlayer)
        {
            View.SetCurrentPlayer(isCurrentPlayer);
        }
    }
} 