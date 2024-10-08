using System;

[Serializable]
public class AlignTag : ParameterTag<TagAlignmentOption>
{
    public AlignTag(string openingTag, string closingTag, RichTag richTag, TagAlignmentOption parameter) : base(openingTag, closingTag, richTag, parameter)
    {
        
    }
    
    protected override string GetOpeningTag()
    {
        return openingTag.Substring(0, openingTag.Length - 1) + parameter.ToString().ToLower() + openingTag.Substring(openingTag.Length - 1, 1);
    }
}
