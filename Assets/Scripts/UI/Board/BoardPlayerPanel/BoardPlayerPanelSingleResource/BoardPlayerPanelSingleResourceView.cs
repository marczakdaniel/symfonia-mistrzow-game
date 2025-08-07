using BrunoMikoski.AnimationSequencer;
using DefaultNamespace.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Board.BoardPlayerPanel.BoardPlayerPanelSingleResource
{
    public class BoardPlayerPanelSingleResourceView : MonoBehaviour
    {
        [SerializeField] 
        private Image tokenImage;

        [SerializeField]
        private TextMeshProUGUI tokenCountText;

        [SerializeField]
        private Image cardImage;

        [SerializeField]
        private TextMeshProUGUI cardCountText;

        [SerializeField]
        private AnimationSequencerController tokenCountAnimation;

        [SerializeField]
        private AnimationSequencerController cardCountAnimation;

        public void Initialize(ResourceType resourceType, int tokenCount, int cardCount)
        {
            tokenImage.color = resourceType.GetColor();
            tokenCountText.text = tokenCount.ToString();
            cardImage.color = resourceType.GetColor();
            cardCountText.text = cardCount.ToString();

            SetCardContainerActive(resourceType != ResourceType.Inspiration);
            UpdateTokenValue(tokenCount);
            UpdateCardValue(cardCount);
        }

        private void SetCardContainerActive(bool active)
        {
            cardImage.gameObject.SetActive(active);
        }

        public void UpdateTokenValue(int tokenCount)
        {
            if (tokenCount == 0)
            {
                tokenCountText.gameObject.SetActive(false);
                tokenImage.color = new Color(tokenImage.color.r, tokenImage.color.g, tokenImage.color.b, 0.7f);
            }
            else
            {
                tokenCountText.gameObject.SetActive(true);
                tokenCountText.text = tokenCount.ToString();
                tokenImage.color = new Color(tokenImage.color.r, tokenImage.color.g, tokenImage.color.b, 1f);
                tokenCountAnimation.ClearPlayingSequence();
                tokenCountAnimation.ResetToInitialState();
                tokenCountAnimation.Play();
            }
        }

        public void UpdateCardValue(int cardCount)
        {
            if (cardCount == 0)
            {
                cardCountText.gameObject.SetActive(false);
                cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0.7f);
            }
            else
            {
                cardCountText.gameObject.SetActive(true);
                cardCountText.text = cardCount.ToString();
                cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 1f);
                cardCountAnimation.ClearPlayingSequence();
                cardCountAnimation.ResetToInitialState();
                cardCountAnimation.Play();
            }
        }
    }
}