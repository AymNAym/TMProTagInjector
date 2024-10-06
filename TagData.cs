using System;
using UnityEngine;

[Serializable]
public abstract class TagData
{
#if UNITY_EDITOR

    public static string OpeningTagProperty => nameof(openingTag);
    public static string ClosingTagProperty => nameof(closingTag);

#endif

    public string OpeningTag => GetOpeningTag();
    public string ClosingTag => GetClosingTag();

    [SerializeField] protected string openingTag;
    [SerializeField] protected string closingTag;
    [SerializeField]
    [HideInInspector] protected RichTag richTag;

    protected virtual string GetOpeningTag() => openingTag;

    protected virtual string GetClosingTag() => closingTag;

    public virtual RichTag GetRichTag() => richTag;

    public virtual bool HasParameter() => false;
    public virtual string ParameterProperty() => string.Empty;
}
