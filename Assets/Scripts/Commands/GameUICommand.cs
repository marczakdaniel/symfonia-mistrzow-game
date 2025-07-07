using Cysharp.Threading.Tasks;
using Events;
using Models;
using UnityEngine;

namespace Command
{
    public class OpenMusicCardDetailsPanelCommand : BaseUICommand
    {
        public override string CommandType => "OpenMusicCardDetailsPanel";
        public string MusicCardId { get; private set; }

        public OpenMusicCardDetailsPanelCommand(string musicCardId, GameModel gameModel) : base(gameModel)
        {
            MusicCardId = musicCardId;
        }

        public override bool Validate()
        {
            return true;
        }   

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new MusicCardDetailsPanelOpenedEvent(MusicCardId));
            return true;
        }
    }

    public class CloseMusicCardDetailsPanelCommand : BaseUICommand
    {
        public override string CommandType => "CloseMusicCardDetailsPanel";
        public CloseMusicCardDetailsPanelCommand(GameModel gameModel) : base(gameModel)
        {

        }

        public override bool Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new MusicCardDetailsPanelClosedEvent());
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