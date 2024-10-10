using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TagInjectorTMPro : TextMeshProUGUI
{
    [SerializeField] private bool buildOnStart = true;
    [SerializeField] private TagDataList globalTags = new();
    [SerializeField] private List<TextTagInfos> tagInfos = new();

    protected override void Start()
    {
        base.Start();

        if (!buildOnStart)
        {
            return;
        }
        
        BuildText();
    }

    public void BuildText()
    {
        var sb = TagInjector.BuildText(tagInfos, globalTags);
        SetText(sb);
    }
}
