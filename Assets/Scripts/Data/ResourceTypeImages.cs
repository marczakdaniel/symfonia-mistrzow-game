using UnityEngine;

namespace DefaultNamespace.Data
{
    [CreateAssetMenu(fileName = "ResourceTypeImages", menuName = "Game/ResourceTypeImages")]
    public class ResourceTypeImages : ScriptableObject
    {
        [SerializeField] private SingleResourceTypeImages[] singleResourceTypeImages;

        public SingleResourceTypeImages[] SingleResourceTypeImages => singleResourceTypeImages;
    }
} 