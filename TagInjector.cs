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
                return new AlignTag("<align=>","</align>", tagEnum, TagAlignmentOption.Left);
            
            case RichTag.AllCaps:
                return new SimpleTag("<allcaps>","</allcaps>", tagEnum);
            
            case RichTag.Alpha:
                return new StringTag("<alpha=#>", "<alpha=#FF>", tagEnum, "FF");
            
            case RichTag.Bold:
                return new SimpleTag("<b>", "</b>", tagEnum);
            
            case RichTag.Color:
                return new ColorTag("<color=#>", "</color>", tagEnum, Color.white);
            
            case RichTag.CharacterSpacing:
                return new EmSpaceTag("<cspace=>", "</cspace>", tagEnum, 1f);
            
            case RichTag.Font:
                return new FontTag("<font=>", "</font>", tagEnum, null);
            
            case RichTag.FontWeight:
                return new IntTag("<font-weight=>", "</font-weight>", tagEnum, 400);
            
            case RichTag.Gradient:
                return new GradientTag("<gradient=>", "</gradient>", tagEnum, null);
            
            case RichTag.Italic:
                return new SimpleTag("<i>", "</i>", tagEnum);
            
            case RichTag.Indent:
                return new PercentageTag("<indent=>", "</indent>", tagEnum, 10f);
            
            case RichTag.LineHeight:
                return new PercentageTag("<line-height=>", "</line-height>", tagEnum, 100f);
            
            case RichTag.LineIndent:
                return new PercentageTag("<line-indent=>", "</line-indent>", tagEnum, 10f);
            
            case RichTag.Link:
                return new StringTag("<link=>", "</link>", tagEnum, "custom link metadata");
            
            case RichTag.Lowercase:
                return new SimpleTag("<lowercase>","</lowercase>", tagEnum);

            case RichTag.Margin:
                return new EmSpaceTag("<margin=>", "</margin>", tagEnum, 5f);
                
            case RichTag.Mark:
                Color defaultColor = Color.yellow;
                defaultColor.a /= 2f;
                return new ColorTag("<mark=#>", "</mark>", tagEnum, defaultColor);
                
            case RichTag.Monospace:
                return new EmSpaceTag("<mspace=>", "</mspace>", tagEnum, 1f);
                
            case RichTag.NoBreak:
                return new SimpleTag("<nobr>", "</nobr>", tagEnum);
            
            case RichTag.NoParse:
                return new SimpleTag("<noparse>", "</noparse>", tagEnum);
                
            case RichTag.PageBreak:
                return new SimpleTag("<page>", string.Empty, tagEnum);
                
            case RichTag.HorizontalPosition:
                return new PercentageTag("<pos=>", "</pos>", tagEnum, 50f);
                
            case RichTag.Rotate:
                return new FloatTag("<rotate=>", "</rotate>", tagEnum, 10f);
                
            case RichTag.Strikethrough:
                return new SimpleTag("<s>", "</s>", tagEnum);
                
            case RichTag.FontSize:
                return new PercentageTag("<size=>", "</size>", tagEnum, 50);
                
            case RichTag.SmallCaps:
                return new SimpleTag("<smallcaps>", "</smallcaps>", tagEnum);
                
            case RichTag.HorizontalSpace:
                return new EmSpaceTag("<space=>", "<space=0em>", tagEnum, 2);
                
            case RichTag.Sprite:
                
            case RichTag.Style:
                
            case RichTag.Subscript:
                
            case RichTag.Superscript:
                
            case RichTag.Underline:
                
            case RichTag.Uppercase:
                
            case RichTag.VerticalOffset:
                
            case RichTag.TextWidth:
                
                break;
        }
        
        throw new ArgumentOutOfRangeException(nameof(tagEnum), tagEnum, null);
    }
}
