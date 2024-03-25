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
        base.OnInspectorGUI();

        var recipeObj = (RecipeScriptableObject)target;
        EditorGUILayout.LabelField("Craft");

        for (int i = 0; i < 3; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < 3; j++)
            {
                recipeObj.craftItems[3*i+j] = EditorGUILayout.ObjectField(recipeObj.craftItems[3*i+j],
                    typeof(CraftItemScriptableObject), false) as CraftItemScriptableObject;
            }
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.LabelField("Result");
        recipeObj.craftResultItem = EditorGUILayout.ObjectField(recipeObj.craftResultItem, typeof(CraftItemScriptableObject), false) as CraftItemScriptableObject;
        recipeObj.resultCount = EditorGUILayout.IntField("Count:", recipeObj.resultCount);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion