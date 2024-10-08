using System;

[Serializable]
public class IntTag : ParameterTag<int>
{
    public IntTag(string openingTag, string closingTag, RichTag richTag, int parameter) : base(openingTag, closingTag, richTag, parameter)
    {
    }
}
