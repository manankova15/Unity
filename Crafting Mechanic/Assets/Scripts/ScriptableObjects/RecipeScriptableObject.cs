using System;
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
    private SerializedProperty craftItems;
    private SerializedProperty craftResultItem;
    private SerializedProperty resultCount;

    private void OnEnable()
    {
        craftItems = serializedObject.FindProperty("craftItems");
        craftResultItem = serializedObject.FindProperty("craftResultItem");
        resultCount = serializedObject.FindProperty("resultCount");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Craft");
        for (int i = 0; i < 3; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < 3; j++)
                EditorGUILayout.ObjectField(craftItems.GetArrayElementAtIndex(3*i+j), GUIContent.none, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth/3-9.5f));
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.LabelField("Result");
        EditorGUILayout.ObjectField(craftResultItem);
        EditorGUILayout.PropertyField(resultCount);
        
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion