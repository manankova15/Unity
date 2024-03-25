using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CraftItem", menuName = "ScriptableObjects/CraftItem", order = 1)]
    public class CraftItemScriptableObject : ScriptableObject
    {
        public string itemName;
        public Sprite sprite;
        public GameObject prefab;
    }
}