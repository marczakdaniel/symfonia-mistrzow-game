using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerAvatarsData", menuName = "Game/PlayerAvatarsData")]
    public class PlayerAvatarsData : ScriptableObject
    {
        [SerializeField] private List<Sprite> avatars;

        public List<Sprite> Avatars => avatars;
    }
}