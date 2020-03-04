using UnityEngine;
[System.Serializable]
public struct ItemDrops
{
    public ItemData item;
    [Range(0, 100)]
    public float changeOfDrop;
    
}
