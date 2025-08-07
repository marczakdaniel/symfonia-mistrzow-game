using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.SelectTokenWindow
{
    public class SelectTokenWindowOpenAnimation : MonoBehaviour
    {
        [SerializeField] private AnimationSequencerController openAnimation;
        [SerializeField] private RectTransform[] tokenOnBoardPositions;
        [SerializeField] private RectTransform[] startPositions;

        public async UniTask PlayOpenAnimation()
        {
            SetPositions();

            await openAnimation.PlayAsync();
        }

        public void SetPositions()
        {
            for (int i = 0; i < tokenOnBoardPositions.Length; i++)
            {
                startPositions[i].position = tokenOnBoardPositions[i].position;
            }

        }
    }
}