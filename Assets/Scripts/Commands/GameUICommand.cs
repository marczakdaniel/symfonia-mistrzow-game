using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using Models;
using UnityEngine;

namespace Command
{
    public class OpenMusicCardDetailsPanelCommand : BaseUICommand
    {
        public override string CommandType => "OpenMusicCardDetailsPanel";
        public string MusicCardId { get; private set; }
        public int Level { get; private set; }
        public int Position { get; private set; }

        public OpenMusicCardDetailsPanelCommand(string musicCardId, int level, int position, GameModel gameModel) : base(gameModel)
        {
            MusicCardId = musicCardId;
            Level = level;
            Position = position;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }   

        public override async UniTask<bool> Execute()
        {
            var canCardBePurchased = true;
            var canCardBeReserved = true;

            await AsyncEventBus.Instance.PublishAndWaitAsync(new MusicCardDetailsPanelOpenedEvent(MusicCardId, Level, Position, canCardBePurchased, canCardBeReserved));
            return true;
        }
    }

    public class CloseMusicCardDetailsPanelCommand : BaseUICommand
    {
        public override string CommandType => "CloseMusicCardDetailsPanel";
        public string MusicCardId { get; private set; }

        public CloseMusicCardDetailsPanelCommand(string musicCardId, GameModel gameModel) : base(gameModel)
        {
            MusicCardId = musicCardId;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new MusicCardDetailsPanelClosedEvent(MusicCardId));
            return true;
        }
    }  

    public class OpenTokenDetailsPanelCommand : BaseUICommand
    {
        public override string CommandType => "OpenTokenDetailsPanel";
        public ResourceType ResourceType { get; private set; }

        public OpenTokenDetailsPanelCommand(ResourceType resourceType, GameModel gameModel) : base(gameModel)
        {
            ResourceType = resourceType;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            gameModel.GetTurnModel().SetState(TurnState.SelectingTokens);
            gameModel.GetTurnModel().AddTokenToSelectedTokens(ResourceType);

            var currentTokenCounts = gameModel.board.TokenResources.GetAllResources();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenDetailsPanelOpenedEvent(ResourceType, currentTokenCounts));
            return true;
        }
    }

    public class CloseTokenDetailsPanelCommand : BaseUICommand
    {
        public override string CommandType => "CloseTokenDetailsPanel";
        public ResourceType ResourceType { get; private set; }
        private readonly GameModel gameModel;
        public CloseTokenDetailsPanelCommand(ResourceType resourceType, GameModel gameModel) : base(gameModel)
        {
            ResourceType = resourceType;
            this.gameModel = gameModel;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            gameModel.GetTurnModel().SetState(TurnState.WaitingForAction);
            gameModel.GetTurnModel().ClearSelectedTokens();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenDetailsPanelClosedEvent(ResourceType));
            return true;
        }
    }

    /*
    public enum GameWindowType
    {
        MusicCardDetailsPanel,
    }

    public class GameWindowParameters
    {
        public GameWindowType WindowType { get; private set; }
        public GameWindowParameters(GameWindowType windowType)
        {
            WindowType = windowType;
    }



    public class OpenGameWindowTypeCommand : BaseUICommand
    {
        public override string CommandType => "OpenGameWindowType";
        public GameWindowType WindowType { get; private set; }

        public OpenGameWindowTypeCommand(GameWindowType windowType,  GameModel gameModel) : base(gameModel)
        {
            WindowType = windowType;
        }

        public override bool Validate()
        {
            return ValidateWindowType(WindowType);
        }

        public override async UniTask<bool> Execute()
        {
            return await ExecuteWindowType(WindowType);
        }

        private bool ValidateWindowType(GameWindowType windowType)
        {
            switch (windowType)
            {
                case GameWindowType.MusicCardDetailsPanel:
                    return true;
                default:
                    return false;
            }
        }

        public UniTask<bool> ExecuteWindowType(GameWindowType windowType)
        {
            switch (windowType)
            {
                case GameWindowType.MusicCardDetailsPanel:
                    return ExecuteMusicCardDetailsPanel();
                default:
                    return UniTask.FromResult(false);
            }
        }

        public async UniTask<bool> ExecuteMusicCardDetailsPanel()
        {
            Debug.Log("Open Music Card Details Panel");
            await UniTask.CompletedTask;
            return true;
        }

    }*/
}