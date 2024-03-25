using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Crafting
{
    public class CraftItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        
        public CraftItemHolder Holder { get; set; }

        [SerializeField] private int count = 1;
        public int Count
        {
            get => count;
            set
            {
                count = value;
            }
        }

        [SerializeField] private CraftItemScriptableObject itemInfo;
        public CraftItemScriptableObject ItemInfo { 
            get => itemInfo;
            set => itemInfo = value;
        }

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI countText;
    
        
    }
}
