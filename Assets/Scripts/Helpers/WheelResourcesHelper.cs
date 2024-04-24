using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct WheelResourcesStruct
{
    public WheelEnum wheelEnum;
    public WheelResources wheelResource;
}

public enum WheelEnum
{
    None,
    Bronze,
    Silver,
    Gold
}

public class WheelResourcesHelper : HelperBase<WheelResourcesHelper>
{
    [SerializeField] List<WheelResourcesStruct> wheelResources;
    private Dictionary<WheelEnum, WheelResources> wheelDictionary = new();

    public void Init()
    {
        foreach (var wheelResource in wheelResources)
        {
            wheelDictionary.Add(wheelResource.wheelEnum, wheelResource.wheelResource);
        }
    }

    public WheelResources GetItemResource(WheelEnum wheelEnum)
    {
        return wheelDictionary[wheelEnum];
    }
}
