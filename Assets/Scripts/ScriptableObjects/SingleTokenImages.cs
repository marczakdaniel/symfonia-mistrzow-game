using UnityEngine;

namespace DefaultNamespace.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SingleTokenImage", menuName = "Create/SingleTokenImageData")]
    public class SingleTokenImages : ScriptableObject
    {
        public TokenType tokenType;
        public Sprite stackImage1;
    }
}