using System;
using UnityEngine;

[Serializable]
public class ColorTag : ParameterTag<Color>
{
    public ColorTag(string openingTag, string closingTag, Color parameter) : base(openingTag, closingTag, parameter)
    {
    }

    protected override string GetOpeningTag()
    {
        return openingTag.Substring(0, openingTag.Length - 1) + ColorUtility.ToHtmlStringRGBA(parameter) + openingTag.Substring(openingTag.Length - 1, 1);
    }
}
