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

        public Sprite GetStackImage(TokenType tokenType, int stackSize)
        {
            var tokenImages = GetTokenImages(tokenType);
            switch (stackSize)
            {
                case 1:
                    return tokenImages.stackImage1;
                case 2:
                    return tokenImages.stackImage2;
                case 3:
                    return tokenImages.stackImage3;
                case > 3:
                    return tokenImages.stackImage3;
                default:
                    return null;
            }
        }

        public Sprite GetTokenCardImage(TokenType tokenType)
        {
            var tokenImages = GetTokenImages(tokenType);
            return tokenImages.tokenCardImage;
        }
    }
}