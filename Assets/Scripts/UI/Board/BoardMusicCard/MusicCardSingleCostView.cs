using System;
using DefaultNamespace.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace UI.Board.BoardMusicCardPanel.BoardMusicCard
{
    public class MusicCardSingleCostView : MonoBehaviour
    {
        [SerializeField] private Image costImage;
        [SerializeField] private TextMeshProUGUI costText;

        public void Setup(ResourceType resourceType, int cost = 0)
        {
            costImage.sprite = resourceType.GetSingleResourceTypeImages().StackImage1;
            costText.text = cost.ToString();
            gameObject.SetActive(cost > 0);
        }
    }
}