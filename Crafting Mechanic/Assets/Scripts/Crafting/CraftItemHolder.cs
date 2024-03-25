using UnityEngine;

namespace Crafting
{
    public class CraftItemHolder : MonoBehaviour
    {
        public CraftManager craftManager;
        
        [SerializeField] private bool isResultHolder;
        public bool IsResultHolder => isResultHolder;
        
        
    }
}
