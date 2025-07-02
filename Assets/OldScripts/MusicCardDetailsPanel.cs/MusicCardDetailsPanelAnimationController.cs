using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace.MusicCardDetailsPanel {
    public class MusicCardDetailsPanelAnimationController : MonoBehaviour {
        [SerializeField] private GameObject panel;
        public async UniTask PlayOpenAnimation() {
            // TODO : Play open animation
            panel.SetActive(true);
            await UniTask.Delay(1);
        }
        public async UniTask PlayCloseAnimation() {
            // TODO : Play close animation
            panel.SetActive(false);
            await UniTask.Delay(1);
        }
    }
}