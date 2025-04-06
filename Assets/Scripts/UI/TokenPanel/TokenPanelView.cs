using UnityEngine;

public class TokenPanelView : MonoBehaviour
{
    [SerializeField] private TokenView[] tokenViews;

    public TokenView GetTokenView(TokenType tokenType)
    {
        foreach (var tokenView in tokenViews)
        {
            if (tokenView.TokenType == tokenType)
            {
                return tokenView;
            }
        }

        return null;
    }
}