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

        public void ApplyCraft() => ApplyCraft(_minCount);

        private (List<int>, List<int>) GetValidationIndexes(IList craftItems) { }
    }
}
