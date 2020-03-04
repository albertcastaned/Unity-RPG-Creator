using UnityEngine;

[CreateAssetMenu(fileName = "New Special Attack", menuName ="Special Attack")]
public class SpecialData : ScriptableObject
{
    public string moveName;
    public int SPcost;
    public string description = "Placeholder description";

    public int value = 10;

    public bool isAHeal = false;

    public StatusEffect statusEffect;
    public MoveDirection moveDirection = MoveDirection.SingleTarget;

    public GameObject specialMoveAnimation;
    
}

