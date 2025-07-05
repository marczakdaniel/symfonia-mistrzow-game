using DefaultNamespace.Data;
using UnityEngine;

namespace UI.Board.BoardMusicCardPanel.BoardMusicCard
{
    public class MusicCardCostView : MonoBehaviour
    {
        [SerializeField] private MusicCardSingleCostView melodySingleCostView;
        [SerializeField] private MusicCardSingleCostView harmonySingleCostView;
        [SerializeField] private MusicCardSingleCostView rhythmSingleCostView;
        [SerializeField] private MusicCardSingleCostView instrumentationSingleCostView;
        [SerializeField] private MusicCardSingleCostView dynamicsSingleCostView;

        public void Setup(ResourceCost cost)
        {
            melodySingleCostView.Setup(ResourceType.Melody, cost.Melody);
            harmonySingleCostView.Setup(ResourceType.Harmony, cost.Harmony);
            rhythmSingleCostView.Setup(ResourceType.Rhythm, cost.Rhythm);
            instrumentationSingleCostView.Setup(ResourceType.Instrumentation, cost.Instrumentation);
            dynamicsSingleCostView.Setup(ResourceType.Dynamics, cost.Dynamics);
        }
    }
}