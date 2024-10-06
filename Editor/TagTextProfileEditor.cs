using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(TagTextProfile))]
public class TagTextProfileEditor : Editor
{
    private SerializedProperty _tagInfos;
    private ReorderableList _list;
    private GUIStyle _deleteStyle;
    private GenericMenu _menu;

    private void OnEnable()
    {
        _tagInfos = serializedObject.FindProperty(TagTextProfile.TagInfosProperty);

        //_list = new ReorderableList(serializedObject, _tagInfos, true, false, false, false)
        //{
        //    drawElementCallback = DrawListItems,
        //    elementHeightCallback = ElementHeightCallback
        //};
    }

    public override void OnInspectorGUI()
    {
        //_deleteStyle ??= new GUIStyle(GUI.skin.button)
        //{
        //    normal = { textColor = Color.red },
        //    hover = { textColor = Color.red },
        //    fontStyle = FontStyle.Bold
        //};

        base.OnInspectorGUI();

        if(GUILayout.Button("Add"))
        {
            var script = (TagTextProfile)target;
            script.Add();
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void InitializeMenu()
    {
        _menu = new GenericMenu();

        foreach(RichTag tag in Enum.GetValues(typeof(RichTag)))
        {
            _menu.AddItem(new GUIContent(tag.ToString()), false, MenuAction, TagInjector.GetTag(tag));
        }
    }

    private void MenuAction(object userData)
    {
        TagData tag = userData as TagData;
        if (null == tag)
        {
            Debug.LogError($"Parameter {userData} is not a tag data.");
            return;
        }

        var script = (TagTextProfile)target;
        //script.tagInfos.Add(tag);
        EditorUtility.SetDirty(target);
    }

    private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.indentLevel++;
        Rect actualRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
        var p = _tagInfos.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(actualRect, p);

        EditorGUI.indentLevel--;

        if (!p.isExpanded)
        {
            return;
        }

        float btnWidth = Mathf.Min(200, rect.width / 2f);
        rect.y += EditorGUI.GetPropertyHeight(p, true) - EditorGUIUtility.singleLineHeight;
        rect.x = (rect.width - btnWidth) / 2f;
        rect.height = EditorGUIUtility.singleLineHeight;
        rect.width = btnWidth;

        if (GUI.Button(rect, "Delete", _deleteStyle))
        {
            //_removeAt = index;
        }
    }

    private float ElementHeightCallback(int index)
    {
        var element = _list.serializedProperty.GetArrayElementAtIndex(index);
        return EditorGUI.GetPropertyHeight(element, true);
    }
}
