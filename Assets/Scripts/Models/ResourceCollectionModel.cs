using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;

namespace Models
{
    public class ResourceCollectionModel
    {
        private Dictionary<ResourceType, int> resources;
        public int TotalCount { get; private set; }

        public ResourceCollectionModel()
        {
            resources = new Dictionary<ResourceType, int>();
            foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
            {
                resources[resourceType] = 0;
            }
            TotalCount = 0;
        }

        public ResourceCollectionModel(ResourceType[] tokens)
        {
            resources = new Dictionary<ResourceType, int>();
            foreach (var token in tokens)
            {
                if (resources.ContainsKey(token))
                {
                    resources[token]++;
                }
                else
                {
                    resources[token] = 1;
                }
            }

            foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
            {
                if (!resources.ContainsKey(resourceType))
                {
                    resources[resourceType] = 0;
                }
            }

            TotalCount = tokens.Length;
        }

        public ResourceCollectionModel(int melody, int harmony, int rhythm, int instrumentation, int dynamics, int inspiration)
        {
            resources = new Dictionary<ResourceType, int>();
            resources[ResourceType.Melody] = melody;
            resources[ResourceType.Harmony] = harmony;
            resources[ResourceType.Rhythm] = rhythm;
            resources[ResourceType.Instrumentation] = instrumentation;
            resources[ResourceType.Dynamics] = dynamics;
            resources[ResourceType.Inspiration] = inspiration;

            TotalCount = melody + harmony + rhythm + instrumentation + dynamics + inspiration;
        }

        public int GetCount(ResourceType resourceType) => resources.GetValueOrDefault(resourceType, 0);
        public bool HasAny(ResourceType resourceType) => GetCount(resourceType) > 0;
        public bool HasEnough(ResourceType resourceType, int count) => GetCount(resourceType) >= count;

        // Buiness logic
        
        public void SetResourceCount(ResourceType resourceType, int count)
        {
            resources[resourceType] = Math.Max(0, count);
            UpdateTotalCount();
        }

        public void AddResource(ResourceType resourceType, int amount)
        {
            if (amount <= 0) return;
            resources[resourceType] += amount;
            UpdateTotalCount();
        }

        public void RemoveResource(ResourceType resourceType, int amount)
        {
            if (amount <= 0) return;
            if (resources[resourceType] < amount) return;
            resources[resourceType] -= amount;
            UpdateTotalCount();
        }

        public void Clear()
        {
            foreach (var key in resources.Keys.ToList())
            {
                resources[key] = 0;
            }
            UpdateTotalCount();
        }

        public int GetTotalResourcese()
        {
            return resources.Values.Sum();
        }

        public ResourceCollectionModel Clone()
        {
            var clone = new ResourceCollectionModel();
            foreach (var key in resources.Keys.ToList())
            {
                clone.SetResourceCount(key, resources[key]);
            }
            return clone;
        }

        public void CopyFrom(ResourceCollectionModel other)
        {
            foreach (var key in resources.Keys.ToList())
            {
                SetResourceCount(key, other.GetCount(key));
            }
        }

        private void UpdateTotalCount()
        {
            TotalCount = GetTotalResourcese();
        }

        public Dictionary<ResourceType, int> GetAllResources()
        {
            return new Dictionary<ResourceType, int>(resources);
        }

        public List<ResourceType> GetNonZeroResourcesTypes()
        {
            return resources.Where(r => r.Value > 0).Select(r => r.Key).ToList();
        }

        public bool HasAll(ResourceCollectionModel other)
        {
            foreach(var r in other.resources)
            {
                if (resources[r.Key] < r.Value) return false;
            }
            return true;
        }

        public void Add(ResourceCollectionModel other)
        {
            foreach (var r in other.resources)
            {
                AddResource(r.Key, r.Value);
            }
        }

        public void Subtract(ResourceCollectionModel other)
        {
            foreach (var r in other.resources)
            {
                RemoveResource(r.Key, r.Value);
            }
        }

        public int HowManychNeedToAddToHaveAll(ResourceCollectionModel other)
        {
            var result = 0;
            foreach (var r in other.resources)
            {
                if (resources[r.Key] < r.Value)
                {
                    result += r.Value - resources[r.Key];
                }
            }
            return result;
        }
    }
}