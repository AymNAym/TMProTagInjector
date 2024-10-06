using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TagInjectorTMPro : TextMeshProUGUI
{
    [SerializeField] private bool buildOnStart = true;
    [SerializeField] private bool automaticSpaceBetweenElements = true;
    [SerializeField] private List<TextTagInfos> tagInfos;

    protected override void Start()
    {
        base.Start();
        
        //tagInfos.Clear();

        //var bold = new TextTagInfos
        //{
        //    text = "This text is bold"
        //};
        //bold.tags.Add(TagInjector.GetTag(RichTag.Bold));

        //var colorTag = (ColorTag)TagInjector.GetTag(RichTag.Color);
        //colorTag.parameter = Color.green;
        //bold.tags.Add(colorTag);
        
        //tagInfos.Add(bold);
        
        if (!buildOnStart)
        {
            return;
        }
        
        BuildText();
    }

    public void BuildText()
    {
        var sb = TagInjector.BuildText(tagInfos);
        SetText(sb);
    }
}
