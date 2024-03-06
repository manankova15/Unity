using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scripts
{
    public class ResourceVisual : MonoBehaviour
    {
        [SerializeField] ResourceBank resourceBank;
        [SerializeField] List<TMP_Text> resourceTexts;

        private readonly List<GameResource> gameResources = new()
            { GameResource.Humans, GameResource.Food, GameResource.Wood, GameResource.Stone, GameResource.Gold };
        
        private void Awake()
        {
            foreach (GameResource res in gameResources)
            {
                resourceBank.GetResource(res).OnValueChanged += value =>
                    resourceTexts[(int)res].text = $"{value}";
            }
        }
    }
}
