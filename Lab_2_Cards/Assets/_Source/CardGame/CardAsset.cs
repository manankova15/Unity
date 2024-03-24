using UnityEngine;

namespace CardGame
{
    /// <summary>
    /// Stores information about the object.
    /// </summary>
    [CreateAssetMenu(fileName = "Asset", menuName = "CardAsset", order=51)]
    public class CardAsset : ScriptableObject
    {
        [SerializeField] private string cardName;
        [SerializeField] private CardColor color;
        
        [SerializeField] internal Sprite onSprite;
        [SerializeField] internal Sprite offSprite;
    }
}
