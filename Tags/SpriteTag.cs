using System;
using UnityEngine;

[Serializable]
public class SpriteTag : ParameterTag<SpriteTagSettings>
{
    public SpriteTag(string openingTag, string closingTag, RichTag richTag, SpriteTagSettings parameter) : base(openingTag, closingTag, richTag, parameter)
    {
    }

    protected override string GetOpeningTag()
    {
        switch (parameter.AccessBy)
        {
            case SpriteTagSettings.AccessMode.Index:
                string byIndexPrefix = string.IsNullOrEmpty(parameter.AssetName) ? $"<sprite index={parameter.SpriteIndex.ToString()}" : $"<sprite=\"{parameter.AssetName}\" index={parameter.SpriteIndex.ToString()}";
                return $"{byIndexPrefix} color=#{ColorUtility.ToHtmlStringRGBA(parameter.Color)}{(parameter.Tint ? " tint=1" : "")}>";
            
            case SpriteTagSettings.AccessMode.Name:
                string byNamePrefix = string.IsNullOrEmpty(parameter.AssetName) ? $"<sprite name=\"{parameter.SpriteName}\"" : $"<sprite=\"{parameter.AssetName}\" name=\"{parameter.SpriteName}\"";
                return $"{byNamePrefix} color=#{ColorUtility.ToHtmlStringRGBA(parameter.Color)}{(parameter.Tint ? " tint=1" : "")}>";
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
