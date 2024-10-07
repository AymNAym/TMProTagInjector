using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTagTextProfile", menuName = "CCLB Studio/TMP Tag Injector/Tag Text Profile")]
public class TagTextProfile : ScriptableObject
{
#if UNITY_EDITOR

    public static string TagInfosProperty => nameof(tagInfos);

#endif

    public List<TextTagInfos> tagInfos;

    public void Add()
    {
        var e = new TextTagInfos
        {
            text = "Hello"
        };

        e.Tags.Add(TagInjector.GetTag(RichTag.Color));
        tagInfos.Add(e);
    }
}
