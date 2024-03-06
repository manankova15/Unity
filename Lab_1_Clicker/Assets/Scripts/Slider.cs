using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class Slider : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private ProductionBuilding builder;
        [SerializeField] private Button button;
        
        private float startPercent = 0.00f;

        public void Fill()
        {
            button.interactable = false;
            int numOfSteps = Convert.ToInt32(builder.ProductionTime * 100);
            float dist = 1.0f / numOfSteps;
            StartCoroutine(FillCoroutine(numOfSteps, dist));
        }

        IEnumerator FillCoroutine(int steps, float dist)
        {
            for (int i = 0; i < steps; ++i)
            {
                image.fillAmount = startPercent;
                startPercent += dist;
                yield return new WaitForEndOfFrame();
            }
            startPercent = 0;
            image.fillAmount = dist;
            button.interactable = true;
            yield return new WaitForEndOfFrame();
        }
    }
}
