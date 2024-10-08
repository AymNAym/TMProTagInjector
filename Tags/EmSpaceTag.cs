using System;
using System.Globalization;

[Serializable]
public class EmSpaceTag : ParameterTag<float>
{
    public EmSpaceTag(string openingTag, string closingTag, RichTag richTag, float parameter) : base(openingTag, closingTag, richTag, parameter)
    {
    }
    
    protected override string GetOpeningTag()
    {
        string value = parameter.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
        return openingTag.Substring(0, openingTag.Length - 1) + value + "em" + openingTag.Substring(openingTag.Length - 1, 1);
    }
}
