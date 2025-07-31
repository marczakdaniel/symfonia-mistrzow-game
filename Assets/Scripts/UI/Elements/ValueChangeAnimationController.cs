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

    private bool isPlaying = false;

    public async UniTask PlayAnimation(int valueChange)
    {
        Debug.Log($"PlayAnimation: {valueChange} {isPlaying}");
        if (isPlaying)
        {
            return;
        }

        isPlaying = true;
        SetupText(valueChange);
        await animationSequencerController.PlayAsync();
        isPlaying = false;
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