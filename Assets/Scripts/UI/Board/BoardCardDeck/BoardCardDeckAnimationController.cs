using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Board.BoardMusicCardPanel.BoardCardDeck
{
    public class BoardCardDeckAnimationController : MonoBehaviour
    {
        [SerializeField] private AnimationSequencerController[] putCardOnBoardAnimations = new AnimationSequencerController[4];
        [SerializeField] private RectTransform[] endPositions = new RectTransform[4];
        [SerializeField] private RectTransform[] animationEndPositions = new RectTransform[4];
        [SerializeField] private AnimationSequencerController[] hideAnimation = new AnimationSequencerController[4];

        public async UniTask PlayPutCardOnBoardAnimation(int position)
        {
            animationEndPositions[position].position = endPositions[position].position;
            await putCardOnBoardAnimations[position].PlayAsync();
        }

        public async UniTask PlayHideAnimation(int position, int delay = 0)
        {
            await UniTask.Delay(delay);
            await hideAnimation[position].PlayAsync();
        }
    }
}