using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class TagInjector
{
    public static StringBuilder BuildText(List<TextTagInfos> tagInfos)
    {
        StringBuilder sb = new StringBuilder();
        
        foreach (var tagInfo in tagInfos)
        {
            foreach (var tag in tagInfo.tags)
            {
                sb.Append(tag.OpeningTag);
            }

            sb.Append(tagInfo.text);
            
            foreach (var tag in tagInfo.tags)
            {
                sb.Append(tag.ClosingTag);
            }
        }

        return sb;
    }

    public static TagData GetTag(RichTag tagEnum)
    {
        switch (tagEnum)
        {
            case RichTag.Align:
                break;
            
            case RichTag.AllCaps:
                break;
            
            case RichTag.Alpha:
                break;
            
            case RichTag.Bold:
                return new SimpleTag("<b>", "</b>");
            
            case RichTag.Color:
                return new ColorTag("<color=>", "</color>", Color.white);
            
            case RichTag.CharacterSpacing:
                break;
            
            case RichTag.Font:
                break;
            
            case RichTag.FontWeight:
                break;
            
            case RichTag.Gradient:
                break;
            
            case RichTag.Italic:
                return new SimpleTag("<i>", "</i>");
            
            case RichTag.Indent:
                break;
            
            case RichTag.LineHeight:
                break;
            
            case RichTag.LineIndent:
                break;
            
            case RichTag.Link:
                break;
            
            case RichTag.Lowercase:
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(tagEnum), tagEnum, null);
        }
        
        throw new ArgumentOutOfRangeException(nameof(tagEnum), tagEnum, null);
    }
}
