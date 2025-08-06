using UnityEngine;

namespace UI.CreatePlayerWindow
{
    public class CreatePlayerWindowViewModel
    {
        public Sprite PlayerAvatar { get; private set; }

        public void SetPlayerAvatar(Sprite playerAvatar)
        {
            PlayerAvatar = playerAvatar;
        }

        public void ResetPlayerAvatar()
        {
            PlayerAvatar = null;
        }
    }
}