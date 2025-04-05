using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField] private CardsRowView[] rowViews;

    public CardsRowView GetCardsRowView(int index)
    {
        if (index < 0 || index >= rowViews.Length)
        {
            return null;
        }

        return rowViews[index];
    }
}