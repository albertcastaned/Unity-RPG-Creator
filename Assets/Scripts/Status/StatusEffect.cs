using UnityEngine;

[System.Serializable]
public struct StatusEffect
{
    public Status status;
    [Range(0, 100)]
    public float chanceOfSuccess;
}
