using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(TagDataList), true)]
public class TagDataListDrawer : PropertyDrawer
{
    private static float StandardHeight = EditorGUIUtility.singleLineHeight;
    private readonly Dictionary<string, DrawerProperties> _initializedDrawers = new();
    private readonly Dictionary<ReorderableList, GenericMenu> _listMenus = new();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var properties = GetRelevantProperties(property);
        properties.height = 0f;
        position.height = StandardHeight;
        
        DrawTagList(ref position, properties, label);
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if(!_initializedDrawers.TryGetValue(property.propertyPath, out var properties))
        {
            return base.GetPropertyHeight(property, label);
        }

        return properties.height;
    }
    
    private void DrawTagList(ref Rect position, DrawerProperties relevantProperties, GUIContent listLabel)
    {
        position.y += EditorGUIUtility.standardVerticalSpacing;
        position.height = StandardHeight;

        var boxRect = new Rect(position);
        boxRect.x -= 15f;
        boxRect.width += 15f;

        relevantProperties.tags.isExpanded = EditorGUI.Foldout(position, relevantProperties.tags.isExpanded, listLabel, true, EditorStyles.foldout);
        
        position.y += EditorGUIUtility.standardVerticalSpacing;
        float h = StandardHeight;
        relevantProperties.height += h;

        if (!relevantProperties.tags.isExpanded)
        {
            boxRect.height = h + EditorGUIUtility.standardVerticalSpacing;
            GUI.Box(boxRect, GUIContent.none, EditorStyles.helpBox);
            return;
        }

        position.y += StandardHeight;
        position.width -= 5f;
        relevantProperties.editorTagList.DoList(position);

        h += relevantProperties.editorTagList.GetHeight();
        position.y += h;
        relevantProperties.height += h + EditorGUIUtility.standardVerticalSpacing;
        
        boxRect.height = h + EditorGUIUtility.standardVerticalSpacing * 2;
        GUI.Box(boxRect, GUIContent.none, EditorStyles.helpBox);
    }

    private DrawerProperties GetRelevantProperties(SerializedProperty property)
    {
        string drawerKey = property.propertyPath;
        if (_initializedDrawers.TryGetValue(drawerKey, out var properties))
        {
            return properties;
        }
        
        var drawerProperties = new DrawerProperties
        {
            tags = property.FindPropertyRelative(TagDataList.TagsProperty),
        };
        
        drawerProperties.editorTagList = new ReorderableList(drawerProperties.tags.serializedObject, drawerProperties.tags, true, true, true, true);
        drawerProperties.editorTagList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (!drawerProperties.tags.isExpanded)
            {
                return;
            }

            var tagProperty = drawerProperties.tags.GetArrayElementAtIndex(index);
            var tagData = (TagData)tagProperty.managedReferenceValue;

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
            var tagData = (TagData)tagProperty.managedReferenceValue;

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
        if(_listMenus.TryGetValue(list, out var initializeMenu))
        {
            return initializeMenu;
        }

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
    
    private class DrawerProperties
    {
        public float height;
        public ReorderableList editorTagList;
        public string displayName;

        public SerializedProperty tags;
    }
    
    private class ListDescriptor
    {
        public SerializedProperty listProperty;
        public RichTag tag;
    }
}
