using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class Lavels : MonoBehaviour
    {
        [SerializeField] private GameResource resource;
        [SerializeField] private ResourceBank bank;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;
        private int _price = 10;

        private void Start()
        {
            text.text = $"Update level of {resource.ToString()} - {Price} coins";
        }

        private int Price
        {
            get => _price * bank.GetResource(bank.GetLevelFromResource(resource)).Value;
        }

        public void UpdateLavel()
        {
            var value = bank.GetResource(resource).Value;
            if (value >= Price)
            {
                bank.GetResource(resource).Value -= Price;
                bank.GetResource(bank.GetLevelFromResource(resource)).Value += 1;
                Fill(Color.green);
            }
            else
            {
                Fill(Color.red);
            }
            text.text = $"Update level of {resource.ToString()} - {Price} coins";
        }
        
        private void Fill(Color color)
        {
            button.interactable = false;
            StartCoroutine(FillCoroutine(300, color));
            button.interactable = true;
        }

        private IEnumerator FillCoroutine(int steps, Color color)
        {
            for (int i = 0; i < steps; ++i)
            {
                if (i % 50 == 0 && i % 100 == 0)
                {
                    button.image.color = color;
                }
                else if (i % 50 == 0 && i % 100 != 0)
                {
                    button.image.color = Color.white;
                }
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
    }
}

