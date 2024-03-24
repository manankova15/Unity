using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace CardGame
{
    /// <summary>
    /// Singleton, that allows starting the game and performing other card manipulations
    /// </summary>
    public class CardGame
    {
        private static CardGame _instance;
        public static CardGame Instance
        {
            get
            {
                return _instance ??= new CardGame();
            }
        }

        private static int _counter = 1;
        public List<CardLayout> Layouts = new();

        private readonly Dictionary<CardInstance, CardView> _cardDictionary = new();
        private List<CardAsset> _initialCards;
        public int HandCapacity;
        private CardLayout _bucketLayout;
        public CardLayout CenterLayout;

        public void Init(List<CardLayout> layouts, List<CardAsset> assets, int capacity, CardLayout center, CardLayout bucket)
        {
            Layouts = layouts;
            _initialCards = assets;
            HandCapacity = capacity;
            CenterLayout = center;
            _bucketLayout = bucket;

            // Start the game by laying out the cards face down
            StartGame();
        }

        private void StartGame()
        {
            // For each player, create a CardInstance for each CardAsset from the initial cards
            foreach (var layout in Layouts)
            {
                foreach (var cardAsset in _initialCards)
                {
                    CreateCard(cardAsset, layout.LayoutId);
                }
            }
        }

        private void CreateCard(CardAsset asset, int layoutNumber)
        {
            // Create a card with LayoutId and calculate CardPosition upon creation as the current number of cards in the layout (zero-based numbering)
            var instance = new CardInstance(asset)
            {
                LayoutId = layoutNumber,
                CardPosition = Layouts[layoutNumber].NowInLayout++
            };
            CreateCardView(instance);
            MoveToLayout(instance, layoutNumber);
        }

        private void CreateCardView(CardInstance instance)
        {
            // Create a GameObject in the scene
            GameObject newCardInstance = new GameObject($"Card {_counter++}");

            // Add components: CardView and Image
            CardView view = newCardInstance.AddComponent<CardView>();
            Image image = newCardInstance.AddComponent<Image>();

            view.Init(instance, image);

            // Add a button component to enable action upon clicking the image
            Button button = newCardInstance.AddComponent<Button>();

            // Add a listener
            button.onClick.AddListener(view.PlayCard);

            // Set its parent in the scene
            newCardInstance.transform.SetParent(Layouts[instance.LayoutId].transform);

            _cardDictionary[instance] = view;
        }

        private void MoveToLayout(CardInstance card, int layoutId)
        {
            int temp = card.LayoutId;
            card.LayoutId = layoutId;

            // Set new parent
            _cardDictionary[card].transform.SetParent(Layouts[layoutId].transform);

            // Recalculate Layouts
            RecalculateLayout(layoutId);
            RecalculateLayout(temp);
        }

        public void MoveToCenter(CardInstance card)
        {
            int temp = card.LayoutId;

            card.LayoutId = CenterLayout.LayoutId;

            // Set new parent
            _cardDictionary[card].transform.SetParent(CenterLayout.transform);

            // Recalculate Layouts
            RecalculateLayout(CenterLayout.LayoutId);
            RecalculateLayout(temp);
        }

        public void MoveToTrash(CardInstance card)
        {
            int temp = card.LayoutId;
            card.LayoutId = _bucketLayout.LayoutId;
            _cardDictionary[card].transform.SetParent(_bucketLayout.transform);

            // Upon moving to trash, remove the button click property from the card
            try
            {
                Button button = _cardDictionary[card].GetComponent<Button>();
                button.enabled = false;
                button.onClick.RemoveAllListeners();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            // Recalculate all layout IDs
            RecalculateLayout(_bucketLayout.LayoutId);
            RecalculateLayout(temp);
        }

        public List<CardView> GetCardsInLayout(int layoutId)
        {
            return _cardDictionary.Where(x => x.Key.LayoutId == layoutId).Select(x => x.Value).ToList();
        }

        private List<CardInstance> GetInstancesInLayout(int layoutId)
        {
            return _cardDictionary.Where(x => x.Key.LayoutId == layoutId).Select(x => x.Key).ToList();
        }

        public void StartTurn()
        {
            // Method to start a turn.
            foreach (var layout in Layouts)
            {
                // Shuffle values in the layout
                ShuffleLayout(layout.LayoutId);

                // Turn the cards face up
                layout.FaceUp = true;

                var cards = GetCardsInLayout(layout.LayoutId);

                // Deal as many cards as possible
                for (int i = 0; i < HandCapacity; ++i)
                {
                    cards[i].StatusOfCard = CardStatus.Hand;
                }
            }
        }

        private void ShuffleLayout(int layoutId)
        {
            var cards = GetInstancesInLayout(layoutId);

            // Create a list of all pairs of cards
            List<(int, int)> pairs = new List<(int, int)>();
            for (int i = 0; i < cards.Count; ++i)
            {
                for (int j = i + 1; j < cards.Count; ++j)
                {
                    pairs.Add((i, j));
                }
            }

            Random rnd = new Random();
            // Set in random order
            pairs = pairs.OrderBy(_ => rnd.Next()).ToList();

            for (var i = 1; i < cards.Count; ++i)
            {
                _cardDictionary[cards[pairs[i].Item1]].transform.SetSiblingIndex(pairs[i].Item2);
            }
        }

        private void RecalculateLayout(int layoutId)
        {
            var games = GetCardsInLayout(layoutId);

            for (int i = 0; i < games.Count; ++i)
            {
                games[i].CardPosition = i;
            }
        }
    }
}
