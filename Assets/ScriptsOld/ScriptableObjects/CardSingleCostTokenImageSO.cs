using UnityEngine;

namespace DefaultNamespace.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCardSingleCostTokenImage", menuName = "Create/SingleCostTokenData")]
    public class CardSingleCostTokenImageSO : ScriptableObject
    {
        public Sprite brownImage;
        public Sprite blueImage;
        public Sprite greenImage;
        public Sprite redImage;
        public Sprite purpleImage;
    }
}