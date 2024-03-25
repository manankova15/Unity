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
                UpdateTextCount();
            }
        }

        [SerializeField] private CraftItemScriptableObject itemInfo;

        public CraftItemScriptableObject ItemInfo
        {
            get => itemInfo;
            set => itemInfo = value;
        }

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI countText;

        private void Start()
        {
            GetComponent<Image>().sprite = ItemInfo.sprite;
            UpdateTextCount();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && !Input.GetKey(KeyCode.LeftShift))
                GrabFullItem();
            else if (eventData.button == PointerEventData.InputButton.Right || Input.GetKey(KeyCode.LeftShift))
            {
                if (Count <= 1)
                    GrabFullItem();
                else
                    SplitItem();

            }

            canvasGroup.blocksRaycasts = false;
            Holder = null;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var mousePos = Input.mousePosition;
            mousePos.z = transform.position.z;
            transform.position = mousePos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var raycastedGO = eventData.pointerCurrentRaycast.gameObject;
            CraftItemHolder holder = null;
            CraftItem craftItem = null;
            canvasGroup.blocksRaycasts = true;

            if (raycastedGO != null)
            {
                holder = raycastedGO.GetComponent<CraftItemHolder>();
                craftItem = raycastedGO.GetComponent<CraftItem>();
            }

            if (holder != null)
                holder.AddItem(this);
            else
            {
                if (craftItem != null)
                    Merge(craftItem);
            }
        }

        public bool DestroyPart(int destroyCount)
        {
            if (destroyCount >= Count)
            {
                Destroy(gameObject);
                return true;
            }

            Count -= destroyCount;
            return false;
        }

        private void UpdateTextCount() => countText.text = Count.ToString();

        private void Merge(CraftItem mergeItem)
        {
            if (mergeItem.Holder != null && mergeItem.Holder.IsResultHolder ||
                mergeItem.ItemInfo.itemName != ItemInfo.itemName)
                return;

            mergeItem.Count += Count;
            if (mergeItem.Holder != null)
                mergeItem.Holder.craftManager.UpdateCraftResult();

            Destroy(gameObject);
        }

        private void GrabFullItem()
        {
            if (Holder != null)
                Holder.RemoveItem();
        }

        private void SplitItem()
        {
            var duplicateCraftItem = Instantiate(gameObject, transform.position, Quaternion.identity, transform.parent)
                .GetComponent<CraftItem>();
            if (Holder != null)
            {
                if (Holder.IsResultHolder)
                {
                    duplicateCraftItem.Count = Count - Holder.craftManager.CurValidRecipe.resultCount;
                    Count = Holder.craftManager.CurValidRecipe.resultCount;
                }
                else
                {
                    duplicateCraftItem.Count = Count / 2 + Count % 2;
                    Count /= 2;
                }

                Holder.CraftItem = duplicateCraftItem;
                duplicateCraftItem.Holder = Holder;
                Holder.SplitItem();
            }
            else
            {
                duplicateCraftItem.Count = Count / 2 + Count % 2;
                Count /= 2;
            }
        }

    }
}