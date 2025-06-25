using System;
using UnityEngine;

namespace DefaultNamespace.Data
{
    [CreateAssetMenu(fileName = "SingleResourceTypeImages", menuName = "Game/SingleResourceTypeImages")]
    public class SingleResourceTypeImages : ScriptableObject
    {
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private Sprite stackImage1;
        [SerializeField] private Sprite stackImage2;
        [SerializeField] private Sprite stackImage3;
        public ResourceType ResourceType => resourceType;
        public Sprite StackImage1 => stackImage1;
        public Sprite StackImage2 => stackImage2;
        public Sprite StackImage3 => stackImage3;
    }
} 