using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class TagInjector
{
    public static StringBuilder BuildText(List<TextTagInfos> tagInfos, TagDataList globalTags, bool automaticSpace)
    {
        StringBuilder sb = new StringBuilder();
        int index = 0;

        foreach (var tag in globalTags.value)
        {
            sb.Append(tag.OpeningTag);
        }
        
        foreach (var tagInfo in tagInfos)
        {
            foreach (var tag in tagInfo.Tags)
            {
                sb.Append(tag.OpeningTag);
            }

            if (automaticSpace && index > 0 && !tagInfos[index - 1].text.EndsWith(' ') && !tagInfo.text.StartsWith(' '))
            {
                sb.Append(" ");
            }
            sb.Append(tagInfo.text);
            index++;
            
            foreach (var tag in tagInfo.Tags)
            {
                sb.Append(tag.ClosingTag);
            }
        }
        
        foreach (var tag in globalTags.value)
        {
            sb.Append(tag.ClosingTag);
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
                return new SimpleTag("<b>", "</b>", tagEnum);
            
            case RichTag.Color:
                return new ColorTag("<color=#>", "</color>", tagEnum, Color.white);
            
            case RichTag.CharacterSpacing:
                break;
            
            case RichTag.Font:
                break;
            
            case RichTag.FontWeight:
                break;
            
            case RichTag.Gradient:
                break;
            
            case RichTag.Italic:
                return new SimpleTag("<i>", "</i>", tagEnum);
            
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
