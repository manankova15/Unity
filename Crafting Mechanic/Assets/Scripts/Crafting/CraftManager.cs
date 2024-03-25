using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;
using Utils;

namespace Crafting
{
    public class CraftManager : MonoBehaviour
    {
        [SerializeField] private RecipeScriptableObject[] recipes;
        [SerializeField] private CraftItemHolder[] holders;
        [SerializeField] private CraftItemHolder resultHolder;

        public RecipeScriptableObject CurValidRecipe { get; private set; }

        private int _minCount = int.MaxValue;

        public void UpdateCraftResult()
        {
            var lastHoldersIndexes = GetValidationIndexes(holders.Select(x => x.CraftItem).ToArray());
            CurValidRecipe = null;
            DestroyResultInstance();

            foreach (var recipe in recipes)
            {
                if (ValidateRecipe(recipe, lastHoldersIndexes))
                {
                    CurValidRecipe = recipe;
                    CreateResultInstance(recipe, _minCount);
                    return;
                }
            }
        }

        private void CreateResultInstance(RecipeScriptableObject recipe, int count)
        {
            var resultCraftItem =
                Instantiate(recipe.craftResultItem.prefab, resultHolder.transform.position, Quaternion.identity,
                    transform.parent).GetComponent<CraftItem>();
            resultHolder.CraftItem = resultCraftItem;
            resultCraftItem.Holder = resultHolder;
            resultCraftItem.ItemInfo = recipe.craftResultItem;
            resultCraftItem.Count = count * recipe.resultCount;
        }

        private void DestroyResultInstance()
        {
            if (resultHolder.CraftItem == null)
                return;

            Destroy(resultHolder.CraftItem.gameObject);
            resultHolder.CraftItem = null;
        }

        public void ApplyCraft(int count)
        {
            foreach (var holder in holders)
                if (holder.CraftItem != null && holder.CraftItem.DestroyPart(count))
                    holder.CraftItem = null;

            if (resultHolder.CraftItem.Count <= 0)
            {
                Destroy(resultHolder.CraftItem);
                resultHolder.CraftItem = null;
            }
        }

        public void ApplyCraft() => ApplyCraft(_minCount);

        private (List<int>, List<int>) GetValidationIndexes(IList craftItems)
        {
            int count = Mathf.RoundToInt(Mathf.Sqrt(craftItems.Count));
            bool[] isEmptyRow = ArrayUtils.InitializeArray(count, true);
            bool[] isEmptyColumn = ArrayUtils.InitializeArray(count, true);
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (craftItems[3 * i + j] != null)
                    {
                        isEmptyColumn[j] = false;
                        isEmptyRow[i] = false;
                    }
                }
            }

            // Don't add empty rows only if there is no items above or below this row.
            var validationRows = new List<int>();
            for (int i = 0; i < count; i++)
            {
                if (isEmptyRow[i])
                {
                    if (i == 0 || i == count - 1)
                        continue;

                    var j = i;
                    bool hasUpper = false;
                    bool hasLower = false;
                    while (j >= 0)
                    {
                        if (!isEmptyRow[j])
                        {
                            hasUpper = true;
                            break;
                        }

                        --j;
                    }

                    j = i;
                    while (j < count)
                    {
                        if (!isEmptyRow[j])
                        {
                            hasLower = true;
                            break;
                        }

                        ++j;
                    }

                    if (hasUpper && hasLower)
                        validationRows.Add(i);
                }
                else
                    validationRows.Add(i);
            }

            // Don't add empty columns only if there is no items above or below this row.
            var validationColumns = new List<int>();
            for (int i = 0; i < count; i++)
            {
                if (isEmptyColumn[i])
                {
                    if (i == 0 || i == count - 1)
                        continue;

                    var j = i;
                    bool hasLeft = false;
                    bool hasRight = false;
                    while (j >= 0)
                    {
                        if (!isEmptyColumn[j])
                        {
                            hasLeft = true;
                            break;
                        }

                        --j;
                    }

                    j = i;
                    while (j < count)
                    {
                        if (!isEmptyColumn[j])
                        {
                            hasRight = true;
                            break;
                        }

                        ++j;
                    }

                    if (hasLeft && hasRight)
                        validationColumns.Add(i);
                }
                else
                    validationColumns.Add(i);
            }

            return (validationRows, validationColumns);
        }

        private bool ValidateRecipe(RecipeScriptableObject recipe,
            (List<int> rowsIndexes, List<int> colIndexes) lastHoldersIndexes)
        {
            (List<int> rowsIndexes, List<int> colIndexes) lastRecipeIndexes = GetValidationIndexes(recipe.craftItems);
            _minCount = int.MaxValue;
            int count = Mathf.RoundToInt(Mathf.Sqrt(recipe.craftItems.Length));
            if (lastHoldersIndexes.rowsIndexes.Count != lastRecipeIndexes.rowsIndexes.Count ||
                lastHoldersIndexes.colIndexes.Count != lastRecipeIndexes.colIndexes.Count)
                return false;

            for (int i = 0; i < lastHoldersIndexes.rowsIndexes.Count; i++)
            {
                for (int j = 0; j < lastHoldersIndexes.colIndexes.Count; j++)
                {
                    int recipeIndex = count * lastRecipeIndexes.rowsIndexes[i] + lastRecipeIndexes.colIndexes[j];
                    int holderIndex = count * lastHoldersIndexes.rowsIndexes[i] + lastHoldersIndexes.colIndexes[j];

                    if (recipe.craftItems[recipeIndex] == null && holders[holderIndex].CraftItem == null)
                        continue;

                    if ((recipe.craftItems[recipeIndex] == null) != (holders[holderIndex].CraftItem == null) ||
                        (recipe.craftItems[recipeIndex].itemName != holders[holderIndex].CraftItem.ItemInfo.itemName))
                        return false;

                    _minCount = Mathf.Min(_minCount, holders[holderIndex].CraftItem.Count);
                }
            }

            return true;
        }
    }
}