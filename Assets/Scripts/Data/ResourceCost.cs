using System;
using UnityEngine;

namespace DefaultNamespace.Data
{
    [Serializable]
    public class ResourceCost
    {
        [SerializeField] private int melody;
        [SerializeField] private int harmony;
        [SerializeField] private int rhythm;
        [SerializeField] private int instrumentation;
        [SerializeField] private int dynamics;

        public int Melody {get => melody; set => melody = value;}
        public int Harmony {get => harmony; set => harmony = value;}
        public int Rhythm {get => rhythm; set => rhythm = value;}
        public int Instrumentation {get => instrumentation; set => instrumentation = value;}
        public int Dynamics {get => dynamics; set => dynamics = value;}

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

        public void SetCost(ResourceType resourceType, int cost)
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