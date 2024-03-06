using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ProductionBuilding : MonoBehaviour
    {
        [SerializeField] private GameResource producedResource;
        [SerializeField] private ResourceBank resourceBank;
        
        private float productionTime = 1f;

        private Coroutine productionCoroutine;
        
        public int CountToAdd
        {
            get => resourceBank.GetResource(resourceBank.GetLevelFromResource(producedResource)).Value;
        }

        public void Increase()
        {
            StartCoroutine(IncreaseCoroutine());
        }
        
        IEnumerator IncreaseCoroutine()
        {
            yield return new WaitForSeconds(ProductionTime);
            resourceBank.ChangeResource(producedResource, CountToAdd);
        }
        
        
        public float ProductionTime
        {
            get => GetSpeedOfProduction();
            set => productionTime = value;
        }
        
        private float GetSpeedOfProduction()
        {
            GameResource level = resourceBank.GetLevelFromResource(producedResource);
            return Math.Max(productionTime * (1.01f - resourceBank.GetResource(level).Value / 100.0f), 0.01f);
        }
    }
}