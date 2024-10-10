using System;
using UnityEngine;

[Serializable]
public class SpriteTagSettings
{
#if UNITY_EDITOR

    public static string AccessByProperty => nameof(accessBy);
    public static string AssetNameProperty => nameof(assetName);
    public static string SpriteIndexProperty => nameof(spriteIndex);
    public static string SpriteNameProperty => nameof(spriteName);
    public static string TintProperty => nameof(tint);
    public static string ColorProperty => nameof(color);
    
#endif
    
    public AccessMode AccessBy => accessBy;
    public string AssetName => assetName;
    public int SpriteIndex => spriteIndex;
    public string SpriteName => spriteName;
    public bool Tint => tint;
    public Color Color => color;

    [SerializeField] private AccessMode accessBy = AccessMode.Name;
    [Tooltip("Leave empty to use the default asset.")]
    [SerializeField] private string assetName = string.Empty;
    [SerializeField] private string spriteName = string.Empty;
    [SerializeField] private int spriteIndex = 0;
    [SerializeField] private bool tint = false;
    [SerializeField] private Color color = Color.white;
    
    public enum AccessMode {Index, Name}
}
