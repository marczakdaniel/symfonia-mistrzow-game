using UI.CreateGameWindow;
using UI.CreatePlayerWindow;
using UI.StartPageWindow;
using UnityEngine;

namespace UI.MenuWindow
{
    public class MenuWindowView : MonoBehaviour
    {
        [SerializeField] private StartPageWindowView startPageWindowView;
        [SerializeField] private CreateGameWindowView createGameWindowView;
        [SerializeField] private CreatePlayerWindowView createPlayerWindowView;

        public StartPageWindowView StartPageWindowView => startPageWindowView;
        public CreateGameWindowView CreateGameWindowView => createGameWindowView;
        public CreatePlayerWindowView CreatePlayerWindowView => createPlayerWindowView;
    }
}