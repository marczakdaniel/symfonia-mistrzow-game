using Cysharp.Threading.Tasks;
using UnityEngine;
using UI.Board.BoardMusicCardPanel.BoardMusicCard;
using TMPro;
using Coffee.UIEffects;

namespace UI.Board.BoardMusicCardPanel.BoardCardDeck
{
    public class BoardCardDeckView : MonoBehaviour
    {
        [SerializeField] private BoardCardDeckAnimationController animationController;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private UIEffect cardLevelEffect;
        [SerializeField] private Color[] cardLevelColors = new Color[3];

        public async UniTask PlayPutCardOnBoardAnimationWithHide(int position, int delay = 0)
        {
            await animationController.PlayPutCardOnBoardAnimation(position);
            animationController.PlayHideAnimation(position, delay).Forget();
        }
        

        public async UniTask PlayHideAnimation(int position)
        {
            await animationController.PlayHideAnimation(position);
        }

        public void SetLevel(int level)
        {
            cardLevelEffect.color = cardLevelColors[level - 1];
            switch (level)
            {
                case 1:
                    levelText.text = "I";
                    break;
                case 2:
                    levelText.text = "II";
                    break;
                case 3:
                    levelText.text = "III";
                    break;
            }
        }
    }
}