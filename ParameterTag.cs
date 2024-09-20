using System;
using UnityEngine;

[Serializable]
public abstract class ParameterTag<T> : TagData
{
    public T parameter;

    protected ParameterTag(string openingTag, string closingTag, T parameter)
    {
        this.openingTag = openingTag;
        this.closingTag = closingTag;
        this.parameter = parameter;
    }

    protected override string GetOpeningTag()
    {
        return openingTag.Substring(0, openingTag.Length - 1) + parameter + openingTag.Substring(openingTag.Length - 1, 1);
    }
}
