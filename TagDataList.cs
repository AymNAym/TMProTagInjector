using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TagDataList
{
    #if UNITY_EDITOR

    public static string TagsProperty => nameof(value);

    #endif
    
    [SerializeReference]
    public List<TagData> value = new();
}
