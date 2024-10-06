using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TagInjectorTMPro))]
public class TagInjectorTMProEditor : TMP_EditorPanelUI
{
    private List<SerializedProperty> _selfProperties;

    protected override void OnEnable()
    {
        base.OnEnable();
        _selfProperties = FetchSelfProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        
        foreach (var property in _selfProperties)
        {
            EditorGUILayout.PropertyField(property);
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
        
        base.OnInspectorGUI();
    }

    private List<SerializedProperty> FetchSelfProperties()
    {
        Type customType = typeof(TagInjectorTMPro);
        FieldInfo[] fields = customType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
        var serializedFields = fields.Where(field => field.IsPublic || field.GetCustomAttribute<SerializeField>() != null);
        List<SerializedProperty> result = new List<SerializedProperty>();

        foreach (var field in serializedFields)
        {
            var property = serializedObject.FindProperty(field.Name);
            if (null == property)
            {
                continue;
            }
            
            result.Add(property);
        }
        
        result.TrimExcess();
        return result;
    }
}
