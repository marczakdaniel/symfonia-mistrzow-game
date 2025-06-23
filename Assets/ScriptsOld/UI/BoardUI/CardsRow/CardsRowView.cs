using TMPro;
using UnityEngine;

public class CardsRowView : MonoBehaviour
{
    [SerializeField] private CardView[] cardViews;
    [SerializeField] private TextMeshProUGUI levelText;

    public void Setup(string level)
    {
        levelText.text = level;
    }

    public CardView GetCardViewAt(int index)
    {
        if (index < 0 || index >= cardViews.Length)
        {
            Debug.LogError($"[RowView] Nieprawidłowy index slotu: {index}");
            return null;
        }

        return cardViews[index];
    }
}