using System;

[Serializable]
public class StringTag : ParameterTag<string>
{
    public StringTag(string openingTag, string closingTag, RichTag richTag, string parameter) : base(openingTag, closingTag, richTag, parameter)
    {
    }
}
