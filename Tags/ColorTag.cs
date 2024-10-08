using System;
using UnityEngine;

[Serializable]
public class ColorTag : ParameterTag<Color>
{
    public ColorTag(string openingTag, string closingTag, RichTag richTag, Color parameter) : base(openingTag, closingTag, richTag, parameter)
    {
    }

    protected override string GetOpeningTag()
    {
        return openingTag.Substring(0, openingTag.Length - 1) + ColorUtility.ToHtmlStringRGBA(parameter) + openingTag.Substring(openingTag.Length - 1, 1);
    }
}
