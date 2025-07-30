using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Elements {

public class ValueChangeAnimationController : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI valueText;
    [SerializeField] 
    private AnimationSequencerController animationSequencerController;
    [SerializeField]
    private Color positiveColor;
    [SerializeField]
    private Color negativeColor;

    public void Initialize(int startValue, int endValue)
    {
        valueText.text = $"{startValue}";
    }

    public async UniTask PlayAnimation(int valueChange)
    {
        SetupText(valueChange);
        await animationSequencerController.PlayAsync();
    }

    private void SetupText(int valueChange)
    {
        if (valueChange > 0)
        {
            valueText.text = $"+{valueChange}";
            valueText.color = positiveColor;
        }
        else
        {
            valueText.text = $"{valueChange}";
            valueText.color = negativeColor;
        }
    }
}

}