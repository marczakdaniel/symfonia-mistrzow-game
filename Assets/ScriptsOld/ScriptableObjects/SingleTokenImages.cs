using UnityEngine;

namespace DefaultNamespace.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SingleTokenImage", menuName = "Create/SingleTokenImageData")]
    public class SingleTokenImages : ScriptableObject
    {
        public TokenType tokenType;
        public Sprite tokenCardImage;
        public Sprite stackImage1;
        public Sprite stackImage2;
        public Sprite stackImage3;
    }
}