using UnityEngine;

public class TokenPanelView : MonoBehaviour
{
    [SerializeField] private TokenView[] tokenViews;

    public TokenView GetTokenView(int index)
    {
        if (index < 0 || index >= tokenViews.Length)
        {
            Debug.LogError($"[TokenPanelView] Nieprawidłowy index tokenu: {index}");
            return null;
        }

        return tokenViews[index];
    }
}