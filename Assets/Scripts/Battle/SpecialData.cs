using UnityEngine;

[CreateAssetMenu(fileName = "New Special Attack", menuName ="Special Attack")]
public class SpecialData : ScriptableObject
{
    public string moveName;
    public int SPcost;
    public string description = "A special attack.";

    public int damage = 10;

    [StringInList("Offensive", "Recovery", "Assist")] public string moveType = "Offense";

    public EntityStatus statusEffect = EntityStatus.None;

    public MoveDirection moveDirection = MoveDirection.SingleTarget;


    
}
