using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    /// <summary>
    /// Class responsible for setting up the game field. Accepts necessary parameters from the inspector and initializes CardGame.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<CardLayout> layouts;
        [SerializeField] private List<CardAsset> assets;
        [SerializeField] private int handCapacity;
        [SerializeField] private CardLayout centerLayout;
        [SerializeField] private CardLayout bucketLayout;

        private void Start()
        {
            // Set the layout IDs for each layout
            int id = 0;
            foreach (var layout in layouts)
            {
                layout.LayoutId = id++;
            }

            centerLayout.LayoutId = id++;
            bucketLayout.LayoutId = id;

            CardGame.Instance.Init(layouts, assets, handCapacity, centerLayout, bucketLayout);
        }

        // Start a turn
        public void StartTurn()
        {
            CardGame.Instance.StartTurn();
        }
    }
}