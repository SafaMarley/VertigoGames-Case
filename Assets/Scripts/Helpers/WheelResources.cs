using System;
using UnityEngine;

[Serializable]
public struct WheelContentStruct
{
    public int itemId;
    public int itemAmount;
}

[CreateAssetMenu]
public class WheelResources : ScriptableObject
{
    public string wheelTitle;
    public string wheelDescription;
    public Sprite wheelSprite;
    public Sprite wheelCellIndicatorSprite;
    public WheelContentStruct[] wheelContents;
}
