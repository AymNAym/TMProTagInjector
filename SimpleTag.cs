using System;

[Serializable]
public class SimpleTag : TagData
{
    public SimpleTag(string openingTag, string closingTag)
    {
        this.openingTag = openingTag;
        this.closingTag = closingTag;
    }
}
