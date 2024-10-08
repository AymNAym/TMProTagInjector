using System;
using TMPro;

[Serializable]
public class GradientTag : ParameterTag<TMP_ColorGradient>
{
    public GradientTag(string openingTag, string closingTag, RichTag richTag, TMP_ColorGradient parameter) : base(openingTag, closingTag, richTag, parameter)
    {
    }
    
    protected override string GetOpeningTag()
    {
        return openingTag.Substring(0, openingTag.Length - 1) + parameter.name + openingTag.Substring(openingTag.Length - 1, 1);
    }
}
