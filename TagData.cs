using System;
using UnityEngine;

[Serializable]
public abstract class TagData
{
    public string OpeningTag => GetOpeningTag();
    public string ClosingTag => GetClosingTag();

    [SerializeField] protected string openingTag;
    [SerializeField] protected string closingTag;

    protected virtual string GetOpeningTag()
    {
        return openingTag;
    }
    
    protected virtual string GetClosingTag()
    {
        return closingTag;
    }
}
