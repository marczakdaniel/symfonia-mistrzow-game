using UnityEngine;
using TMPro;

namespace UI.ReturnTokenWindow
{
    public class ReturnTokenAllTokenCountElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI allTokenCountText;
        [SerializeField] private Color warningColor;
        [SerializeField] private Color normalColor;

        public void SetAllTokenCount(int allTokenCount)
        {
            allTokenCountText.SetText(allTokenCount.ToString());
            if (allTokenCount > 10)
            {
                allTokenCountText.color = warningColor;
            }
            else
            {
                allTokenCountText.color = normalColor;
            }
        }
    }
}