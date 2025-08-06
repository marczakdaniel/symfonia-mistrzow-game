using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Elements;
using R3;
using TMPro;
using UnityEngine;

namespace UI.CreatePlayerWindow
{
    public class CreatePlayerWindowView : MonoBehaviour
    {
        public Subject<string> OnAddPlayerButtonClicked { get; private set; } = new Subject<string>();
        public Subject<Unit> OnCloseButtonClicked { get; private set; } = new Subject<Unit>();
        public Subject<(Sprite, int)> OnAvatarClicked { get; private set; } = new Subject<(Sprite, int)>();

        [SerializeField] 
        private TMP_InputField playerNameInputField;

        [SerializeField]
        private ButtonElement addPlayerButton;

        [SerializeField]
        private ButtonElement closeButton;

        [SerializeField]
        private AnimationSequencerController openAnimation;

        [SerializeField]
        private AnimationSequencerController closeAnimation;

        [SerializeField]
        private RectTransform avatarsContainer;

        [SerializeField]
        private PlayerImageElement playerImageElementPrefab;

        private List<PlayerImageElement> playerImageElements = new List<PlayerImageElement>();


        public void Awake()
        {
            addPlayerButton.OnClick.Subscribe(_ => OnAddPlayerButtonClicked.OnNext(playerNameInputField.text)).AddTo(this);
            closeButton.OnClick.Subscribe(_ => OnCloseButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }

        public async UniTask PlayOpenAnimation()
        {
            await openAnimation.PlayAsync();
        }

        public async UniTask PlayCloseAnimation()
        {
            playerNameInputField.text = "";
            await closeAnimation.PlayAsync();
        }

        public void SetupAvatars(List<Sprite> avatars)
        {
            foreach (var playerImageElement in playerImageElements)
            {
                Destroy(playerImageElement.gameObject);
                playerImageElement.OnClick.Dispose();
            }
            playerImageElements.Clear();

            for (int i = 0; i < avatars.Count; i++)
            {
                var avatar = avatars[i];
                var playerImageElement = Instantiate(playerImageElementPrefab, avatarsContainer.transform);
                playerImageElement.Setup(avatar, i);
                playerImageElements.Add(playerImageElement);
                playerImageElement.OnClick.Subscribe(x => OnAvatarClicked.OnNext(x)).AddTo(this);
            }
            ResetSelectedAvatar();
        }

        public void ResetSelectedAvatar()
        {
            foreach (var playerImageElement in playerImageElements)
            {
                playerImageElement.SetSelected(false);
            }
        }

        public void SetSelectedAvatar(int index)
        {
            Debug.Log($"SetSelectedAvatar: {index}");
            foreach (var playerImageElement in playerImageElements)
            {
                if (playerImageElement.Index == index)
                {
                    playerImageElement.SetSelected(true);
                }
            }
        }
    }
}