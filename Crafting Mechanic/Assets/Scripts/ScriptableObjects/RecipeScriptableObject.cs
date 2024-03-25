using UnityEngine;
#if UNITY_EDITOR
using ScriptableObjects;
using UnityEditor;
#endif

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]
    public class RecipeScriptableObject : ScriptableObject
    {
        [HideInInspector] public CraftItemScriptableObject[] craftItems = new CraftItemScriptableObject[9];
        [HideInInspector] public CraftItemScriptableObject craftResultItem;
        [HideInInspector] public int resultCount = 1;
    }
}

#region EditorGUI
#if UNITY_EDITOR
[CustomEditor(typeof(RecipeScriptableObject))]
public class RecipeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
    }
}
#endif
#endregion

