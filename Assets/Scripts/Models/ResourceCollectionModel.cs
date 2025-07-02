using System;
using System.Collections.Generic;
using DefaultNamespace.Data;

namespace Models
{
    public class ResourceCollectionModel
    {
        private Dictionary<ResourceType, int> resources;

        public ResourceCollectionModel()
        {
            resources = new Dictionary<ResourceType, int>();
            foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
            {
                resources[resourceType] = 0;
            }
        }

        public int GetCount(ResourceType resourceType) => resources.GetValueOrDefault(resourceType, 0);
        public bool HasAny(ResourceType resourceType) => GetCount(resourceType) > 0;
        public bool HasEnough(ResourceType resourceType, int count) => GetCount(resourceType) >= count;

        // Buiness logic
        

    }
}