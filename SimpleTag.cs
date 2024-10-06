using System;

[Serializable]
public class SimpleTag : TagData
{
    public SimpleTag(string openingTag, string closingTag, RichTag richTag)
    {
        this.openingTag = openingTag;
        this.closingTag = closingTag;
        this.richTag = richTag;
    }
}
