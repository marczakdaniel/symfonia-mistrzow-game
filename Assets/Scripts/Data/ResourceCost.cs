using System;
using UnityEngine;

namespace DefaultNamespace.Data
{
    // Read-only interface for UI layer
    public interface IResourceCostReader
    {
        int Melody { get; }
        int Harmony { get; }
        int Rhythm { get; }
        int Instrumentation { get; }
        int Dynamics { get; }
        int GetCost(ResourceType resourceType);
        int TotalCost();
    }

    [Serializable]
    public class ResourceCost : IResourceCostReader
    {
        [SerializeField] private int melody;
        [SerializeField] private int harmony;
        [SerializeField] private int rhythm;
        [SerializeField] private int instrumentation;
        [SerializeField] private int dynamics;

        // Read-only interface implementation
        public int Melody => melody;
        public int Harmony => harmony;
        public int Rhythm => rhythm;
        public int Instrumentation => instrumentation;
        public int Dynamics => dynamics;

        // Setters only for business logic layer
        internal int MelodyInternal {get => melody; set => melody = value;}
        internal int HarmonyInternal {get => harmony; set => harmony = value;}
        internal int RhythmInternal {get => rhythm; set => rhythm = value;}
        internal int InstrumentationInternal {get => instrumentation; set => instrumentation = value;}
        internal int DynamicsInternal {get => dynamics; set => dynamics = value;}

        public ResourceCost(int melody, int harmony, int rhythm, int instrumentation, int dynamics)
        {
            this.melody = melody;
            this.harmony = harmony;
            this.rhythm = rhythm;
            this.instrumentation = instrumentation;
            this.dynamics = dynamics;
        }

        public int GetCost(ResourceType resourceType)
        {
            return resourceType switch
            {
                ResourceType.Melody => melody,
                ResourceType.Harmony => harmony,
                ResourceType.Rhythm => rhythm,
                ResourceType.Instrumentation => instrumentation,
                ResourceType.Dynamics => dynamics,
                _ => 0
            };
        }

        internal void SetCost(ResourceType resourceType, int cost)
        {
            switch (resourceType)
            {
                case ResourceType.Melody: melody = cost; break;
                case ResourceType.Harmony: harmony = cost; break;
                case ResourceType.Rhythm: rhythm = cost; break;
                case ResourceType.Instrumentation: instrumentation = cost; break;
                case ResourceType.Dynamics: dynamics = cost; break;
                default: break;
            }
        }

        public int TotalCost()
        {
            return melody + harmony + rhythm + instrumentation + dynamics;
        }
    }
}