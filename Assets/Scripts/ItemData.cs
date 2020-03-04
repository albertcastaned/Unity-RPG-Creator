using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    public string itemName;

    public Sprite image;

    public string description;
    public int value;

    public bool isAHeal = false;

    public Status statusEffect;

    public MoveDirection moveDirection = MoveDirection.SingleTarget;

}
