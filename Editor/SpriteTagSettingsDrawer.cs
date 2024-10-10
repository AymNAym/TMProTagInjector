using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpriteTagSettings))]
public class SpriteTagSettingsDrawer : PropertyDrawer
{
    private readonly Dictionary<string, DrawerProperties> _initializedDrawers = new();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var properties = GetRelevantProperties(property);
        properties.height = 0f;
        
        DrawStandardProperty(ref position, properties.assetName, properties);
        DrawStandardProperty(ref position, properties.accessBy, properties);
        SpriteTagSettings.AccessMode mode = (SpriteTagSettings.AccessMode)properties.accessBy.enumValueIndex;
        DrawStandardProperty(ref position, mode == SpriteTagSettings.AccessMode.Index ? properties.spriteIndex : properties.spriteName, properties);
        DrawStandardProperty(ref position, properties.tint, properties);
        DrawStandardProperty(ref position, properties.color, properties);
    }

    private DrawerProperties GetRelevantProperties(SerializedProperty property)
    {
        if (_initializedDrawers.TryGetValue(property.propertyPath, out var properties))
        {
            return properties;
        }

        var newProperties = new DrawerProperties
        {
            height = 0f,
            accessBy = property.FindPropertyRelative(SpriteTagSettings.AccessByProperty),
            assetName = property.FindPropertyRelative(SpriteTagSettings.AssetNameProperty),
            spriteIndex = property.FindPropertyRelative(SpriteTagSettings.SpriteIndexProperty),
            spriteName = property.FindPropertyRelative(SpriteTagSettings.SpriteNameProperty),
            tint = property.FindPropertyRelative(SpriteTagSettings.TintProperty),
            color = property.FindPropertyRelative(SpriteTagSettings.ColorProperty)
        };

        _initializedDrawers.Add(property.propertyPath, newProperties);
        return newProperties;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if(!_initializedDrawers.TryGetValue(property.propertyPath, out var properties))
        {
            return base.GetPropertyHeight(property, label);
        }
        
        return properties.height;
    }
    
    private void DrawStandardProperty(ref Rect position, SerializedProperty property, DrawerProperties relevantProperties)
    {
        float h = EditorGUI.GetPropertyHeight(property);
        position.y += EditorGUIUtility.standardVerticalSpacing;
        position.height = h;

        EditorGUI.PropertyField(position, property);

        position.y += h;
        relevantProperties.height += h + EditorGUIUtility.standardVerticalSpacing;
    }
    
    private class DrawerProperties
    {
        public float height;
        public SerializedProperty accessBy;
        public SerializedProperty assetName;
        public SerializedProperty spriteIndex;
        public SerializedProperty spriteName;
        public SerializedProperty tint;
        public SerializedProperty color;
    }
}
