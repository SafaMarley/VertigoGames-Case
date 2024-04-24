using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct ItemResourcesStruct
{
    public ItemEnum itemEnum;
    public ItemResources itemResource;
}

public enum ItemEnum
{
    Cash = 0,
    Glasses,
    Cap,
    Gold,
    Chest,
    Bayonet,
    Grenade,
    Sniper
}

public class ItemResourcesHelper : HelperBase<ItemResourcesHelper>
{
    [SerializeField] List<ItemResourcesStruct> itemResources;
    private Dictionary<ItemEnum, ItemResources> itemDictionary = new();

    public void Init()
    {
        foreach (var itemResource in itemResources)
        {
            itemDictionary.Add(itemResource.itemEnum, itemResource.itemResource);
        }
    }

    public ItemResources GetItemResource(ItemEnum itemEnum)
    {
        return itemDictionary[itemEnum];
    }
}
