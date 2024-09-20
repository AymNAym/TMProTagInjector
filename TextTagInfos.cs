using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextTagInfos
{
    [SerializeReference] public List<TagData> tags = new();
    [TextArea(3, 8)]
    public string text = string.Empty;
}
