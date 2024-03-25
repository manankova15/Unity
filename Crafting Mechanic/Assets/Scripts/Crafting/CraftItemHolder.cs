using UnityEngine;

namespace Crafting
{
    public class CraftItemHolder : MonoBehaviour
    {
        public CraftManager craftManager;
        
        [SerializeField] private bool isResultHolder;
        public bool IsResultHolder => isResultHolder;
        
        public CraftItem CraftItem { get; set; }
    
        public void AddItem(CraftItem addItem)
        {
            if (IsResultHolder) 
                return;

            if (CraftItem != null)
            {
                if (CraftItem.ItemInfo.itemName != addItem.ItemInfo.itemName)
                    return;
            
                Merge(addItem);
            }
            else
            {
                CraftItem = addItem;
            
                addItem.transform.position = transform.position;
                addItem.Holder = this; 
            }
        
            craftManager.UpdateCraftResult();
        }

        public void RemoveItem()
        {
            if (IsResultHolder)
            {
                craftManager.ApplyCraft();
                CraftItem = null;
            }
            else
            {
                CraftItem = null;
                craftManager.UpdateCraftResult();  
            }
        }

        public void SplitItem()
        {
            if (IsResultHolder)
                craftManager.ApplyCraft(1);
            else
                craftManager.UpdateCraftResult();  
        }
        private void Merge(CraftItem mergeItem)
        {
            CraftItem.Count += mergeItem.Count;
            Destroy(mergeItem.gameObject);
        }
    }
}