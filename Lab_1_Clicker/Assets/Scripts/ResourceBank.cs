using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ResourceBank : MonoBehaviour
    {
        private Dictionary<GameResource, ObservableInt> resources = new();

        public ResourceBank()
        {
            foreach (GameResource resource in Enum.GetValues(typeof(GameResource)))
            {
                if (resource == GameResource.Humans || resource == GameResource.Food || 
                    resource == GameResource.Wood || resource == GameResource.Stone || 
                    resource == GameResource.Gold)
                {
                    resources.Add(resource, new ObservableInt(0));
                }
                else if (resource == GameResource.HumansLvl || resource == GameResource.FoodLvl ||
                         resource == GameResource.WoodLvl || resource == GameResource.StoneLvl ||
                         resource == GameResource.GoldLvl)
                {
                    resources.Add(resource, new ObservableInt(1));
                }
            }
        }

        public void ChangeResource(GameResource r, int v)
        {
            if (!resources.ContainsKey(r))
            {
                resources[r] = new ObservableInt(v);
            }
            else
            {
                resources[r].Value += v;
            }
        }

        public ObservableInt GetResource(GameResource r)
        {
            if (resources.ContainsKey(r))
            {
                return resources[r];
            }
            
            resources[r].Value = 0;
            return resources[r];

        }
        
        public GameResource GetLevelFromResource(GameResource resource)
        {
            switch (resource)
            {
                case GameResource.Food:
                    return GameResource.FoodLvl;
                case GameResource.Gold:
                    return GameResource.GoldLvl;
                case GameResource.Humans:
                    return GameResource.HumansLvl;
                case GameResource.Wood:
                    return GameResource.WoodLvl;
                case GameResource.Stone:
                    return GameResource.StoneLvl;
                default:
                    return GameResource.FoodLvl;
            }
        }
    }
}