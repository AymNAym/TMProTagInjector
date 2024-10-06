using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(TextTagInfos), true)]
public class TextTagInfosDrawer : PropertyDrawer
{
    private readonly Dictionary<string, DrawerProperties> _initializedDrawers = new();
    private readonly Dictionary<ReorderableList, GenericMenu> _listMenus = new();
    private static float StandardHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

    private DrawerProperties Initialize(SerializedProperty property)
    {
        string drawerKey = property.propertyPath;
        if (_initializedDrawers.TryGetValue(drawerKey, out var properties))
        {
            return properties;
        }

        var drawerProperties = new DrawerProperties
        {
            tags = property.FindPropertyRelative(TextTagInfos.TagsProperty),
            text = property.FindPropertyRelative(TextTagInfos.TextProperty),
        };

        drawerProperties.editorTagList = new ReorderableList(drawerProperties.tags.serializedObject, drawerProperties.tags, true, true, true, true);
        drawerProperties.editorTagList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (!drawerProperties.tags.isExpanded)
            {
                return;
            }

            var tagProperty = drawerProperties.tags.GetArrayElementAtIndex(index);
            var tagData = ((TagData)tagProperty.managedReferenceValue);

            if(tagData == null)
            {
                return;
            }

            string displayName = tagData.GetRichTag().ToString();
            rect.height = StandardHeight;

            if(!tagData.HasParameter())
            {
                EditorGUI.LabelField(rect, new GUIContent(displayName));
                return;
            }

            EditorGUI.indentLevel++;
            tagProperty.isExpanded = EditorGUI.Foldout(rect, tagProperty.isExpanded, new GUIContent(displayName), true);

            if (!tagProperty.isExpanded)
            {
                EditorGUI.indentLevel--;
                return;
            }

            EditorGUI.indentLevel++;

            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(rect, tagProperty.FindPropertyRelative(tagData.ParameterProperty()));

            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        };

        drawerProperties.editorTagList.elementHeightCallback = (int index) =>
        {
            if (!drawerProperties.tags.isExpanded)
            {
                return 0f;
            }

            var tagProperty = drawerProperties.tags.GetArrayElementAtIndex(index);
            var tagData = ((TagData)tagProperty.managedReferenceValue);

            if(tagData == null)
            {
                return 0f;
            }

            if(!tagData.HasParameter())
            {
                return StandardHeight;
            }

            return tagProperty.isExpanded ? 2 * StandardHeight : StandardHeight;
        };


        //drawerProperties.editorTagList.drawHeaderCallback = (Rect rect) =>
        //{
        //    EditorGUI.indentLevel++;
        //    drawerProperties.tags.isExpanded = EditorGUI.Foldout(rect, drawerProperties.tags.isExpanded, new GUIContent(drawerProperties.tags.displayName), true);
        //    EditorGUI.indentLevel--;
        //};

        drawerProperties.editorTagList.headerHeight = 0f;

        drawerProperties.editorTagList.onAddCallback = (ReorderableList list) =>
        {
            var menu = InitializeMenu(list, drawerProperties.tags);
            menu.ShowAsContext();
        };

        drawerProperties.editorTagList.onRemoveCallback = (ReorderableList list) =>
        {
            DeleteTag(list, drawerProperties.tags);
        };

        _initializedDrawers.Add(property.propertyPath, drawerProperties);
        return drawerProperties;
    }

    private GenericMenu InitializeMenu(ReorderableList list, SerializedProperty realList)
    {
        if(_listMenus.ContainsKey(list))
        {
            Debug.Log("Generic menu already exist");
            return _listMenus[list];
        }

        Debug.Log("Creating new generic menu");
        var menu = new GenericMenu();

        var arr = Enum.GetValues(typeof(RichTag));
        foreach(var e in arr)
        {
            menu.AddItem(new GUIContent(ObjectNames.NicifyVariableName(e.ToString())), false, MenuAction, new ListDescriptor { listProperty = realList, tag = (RichTag)e });
        }

        _listMenus[list] = menu;
        return menu;
    }

    private void MenuAction(object userData)
    {
        var listData = (ListDescriptor)userData;
        listData.listProperty.arraySize++;
        var added = listData.listProperty.GetArrayElementAtIndex(listData.listProperty.arraySize - 1);
        added.managedReferenceValue = TagInjector.GetTag(listData.tag);

        listData.listProperty.serializedObject.ApplyModifiedProperties();
    }

    private void DeleteTag(ReorderableList list, SerializedProperty tagsProperty)
    {
        if(list.selectedIndices.Count <= 0)
        {
            tagsProperty.DeleteArrayElementAtIndex(tagsProperty.arraySize - 1);
            return;
        }

        foreach(int index in list.selectedIndices)
        {
            tagsProperty.DeleteArrayElementAtIndex(index);
        }
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var properties = Initialize(property);
        properties.height = 0f;
        position.height = StandardHeight;

        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, new GUIContent(property.displayName), true);

        properties.height += StandardHeight;
        position.y += StandardHeight;

        if (!property.isExpanded)
        {
            return;
        }

        DrawStandardPorperty(ref position, properties.text, properties);
        //DrawStandardPorperty(ref position, properties.tags, properties);

        //properties.editorTagList.DoLayoutList();
        DrawTagList(ref position, properties);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if(!_initializedDrawers.TryGetValue(property.propertyPath, out var properties))
        {
            return base.GetPropertyHeight(property,label);
        }

        return properties.height;
    }

    private void DrawStandardPorperty(ref Rect position, SerializedProperty property, DrawerProperties relevantProperties)
    {
        float h = EditorGUI.GetPropertyHeight(property);
        position.y += EditorGUIUtility.standardVerticalSpacing;
        position.height = h;

        EditorGUI.PropertyField(position, property);

        position.y += h;
        relevantProperties.height += h + EditorGUIUtility.standardVerticalSpacing;
    }

    private void DrawTagList(ref Rect position, DrawerProperties relevantProperties)
    {
        position.y += EditorGUIUtility.standardVerticalSpacing;
        position.height = StandardHeight;
        relevantProperties.tags.isExpanded = EditorGUI.Foldout(position, relevantProperties.tags.isExpanded, "Tags", true, EditorStyles.foldoutHeader);
        float h = StandardHeight;
        relevantProperties.height += h;

        if (!relevantProperties.tags.isExpanded)
        {
            return;
        }

        position.y += StandardHeight;
        relevantProperties.editorTagList.DoList(position);

        h += relevantProperties.editorTagList.GetHeight();
        position.y += h;
        relevantProperties.height += h + EditorGUIUtility.standardVerticalSpacing;
    }

    private class DrawerProperties
    {
        public float height;
        public ReorderableList editorTagList;
        public string displayName;

        public SerializedProperty tags;
        public SerializedProperty text;
    }

    private class ListDescriptor
    {
        public SerializedProperty listProperty;
        public RichTag tag;
    }
}
