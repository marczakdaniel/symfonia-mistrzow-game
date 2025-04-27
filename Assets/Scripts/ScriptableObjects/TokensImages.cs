using System.Linq;
using UnityEngine;

namespace DefaultNamespace.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TokensImages", menuName = "Create/TokensImagesData")]
    public class TokensImagesSO : ScriptableObject
    {
        public SingleTokenImages[] singleTokenImages;

        public SingleTokenImages GetTokenImages(TokenType tokenType)
        {
            return singleTokenImages.First(t => t.tokenType == tokenType);
        }
    }
}