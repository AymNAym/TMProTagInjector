using System;

[Serializable]
public class FloatTag : ParameterTag<float>
{
    public FloatTag(string openingTag, string closingTag, RichTag richTag, float parameter) : base(openingTag, closingTag, richTag, parameter)
    {
    }
}
