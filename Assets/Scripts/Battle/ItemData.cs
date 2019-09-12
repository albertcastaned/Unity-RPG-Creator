using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public int gainAmount;

    public ItemType effect = ItemType.Heal;
}
