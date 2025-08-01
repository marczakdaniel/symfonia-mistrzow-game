using System;
using Assets.Scripts.Data;
using Coffee.UIEffects;
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

        public void Initialize(ConcertCardData cardData, ConcertCardState cardState)
        {
            image.sprite = cardData.Image;
            pointsText.text = cardData.Points.ToString();

            SetCardState(cardState);

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
        }

        private void SetCardState(ConcertCardState cardState)
        {
            switch (cardState)
            {
                case ConcertCardState.Available:
                    imageEffect.toneIntensity = 0;
                    break;
                case ConcertCardState.ReadyToClaim:
                case ConcertCardState.Claimed:
                    imageEffect.toneIntensity = 0.5f;
                    break;
            }
        }
    }
}