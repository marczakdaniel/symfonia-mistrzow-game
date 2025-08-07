using System;
using System.Threading.Tasks;
using Assets.Scripts.Data;
using BrunoMikoski.AnimationSequencer;
using Coffee.UIEffects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class ConcertCardElement : MonoBehaviour
    {
        [SerializeField] 
        private Image image;
        
        [SerializeField]
        private TextMeshProUGUI pointsText;

        [SerializeField]
        private CardRequirementContainer[] cardRequirement = new CardRequirementContainer[3];

        [SerializeField]
        private UIEffect imageEffect;

        [SerializeField]
        private AnimationSequencerController claimAnimation;

        [SerializeField]
        private Image ownerAvatar;

        private bool canClaim = false;

        public void Initialize(ConcertCardData cardData, ConcertCardState cardState, Sprite avatar)
        {
            image.sprite = cardData.Image;
            pointsText.text = cardData.Points.ToString();

            SetCardState(cardState);

            ownerAvatar.sprite = avatar;

            var requirementIndex = 0;
            var requirements = cardData.GetRequirements();
            foreach (var resourceType in Enum.GetValues(typeof(ResourceType)))
            {
                if(requirementIndex >= cardRequirement.Length)
                {
                    break;
                }

                if (requirements.ContainsKey((ResourceType)resourceType) && requirements[(ResourceType)resourceType] > 0)
                {
                    cardRequirement[requirementIndex].Initialize((ResourceType)resourceType, requirements[(ResourceType)resourceType]);
                    cardRequirement[requirementIndex].gameObject.SetActive(true);
                    requirementIndex++;
                }
            }

            for (int i = requirementIndex; i < cardRequirement.Length; i++)
            {
                cardRequirement[i].gameObject.SetActive(false);
            }

            canClaim = cardState == ConcertCardState.ReadyToClaim;
        }

        public async UniTask PlayClaimAnimation()
        {
            if (!canClaim)
            {
                return;
            }

            await claimAnimation.PlayAsync();
        }

        private void SetCardState(ConcertCardState cardState)
        {
            switch (cardState)
            {
                case ConcertCardState.ReadyToClaim:
                case ConcertCardState.Available:
                    imageEffect.toneIntensity = 1f;
                    break;
                case ConcertCardState.Claimed:
                    imageEffect.toneIntensity = 0f;
                    break;
            }
        }
    }
}