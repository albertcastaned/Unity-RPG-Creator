using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Command
{

    private TypeOfCommand commandType;
    public TypeOfCommand CommandType { get => commandType; set => commandType = value; }

    private BattleEntity caster;
    private List<BattleEntity> targets;

    private int order;
    private bool miss;
    private bool dodge;
    private bool critical;

    private int moveValue;

    private EntityStatus commandStatus;
    //TODO Add animations
    public List<BattleEntity> Targets { get => targets; set => targets = value; }

    public BattleEntity Caster { get => caster; set => caster = value; }

    private ItemType itemType;
    public ItemType ItemType { get => ItemType; set => itemType = value; }
    public int Value { get => moveValue; set => moveValue = value; }
    public int Order { get => order; set => order = value; }
    public bool Miss { get => miss; set => miss = value; }
    public bool Dodge { get => dodge; set => dodge = value; }
    public bool Critical { get => critical; set => critical = value; }


    //Constructor used by Special Move
    public Command(SpecialData specialMove, BattleEntity newCaster, List<BattleEntity> newTarget)
    {
        caster = newCaster;
        targets = newTarget;

        commandType = TypeOfCommand.Special;

        moveValue = specialMove.damage;
        commandStatus = specialMove.statusEffect;
        Order = newCaster.Speed;
    }

    //Constructor used by Item
    public Command(ItemData item, BattleEntity newCaster, List<BattleEntity> newTarget)
    {
        caster = newCaster;
        targets = newTarget;
        commandType = TypeOfCommand.Item;
        Order = newCaster.Speed;

        moveValue = item.gainAmount;
        itemType = item.effect;
    }

    //Constructor used by regular attack
    public Command(BattleEntity newCaster, List<BattleEntity> newTarget)
    {
        commandType = TypeOfCommand.Melee;

        caster = newCaster;
        targets = newTarget;
        
        //TODO Set probability to weapon type or status effect
        int missProbability = Random.Range(1, 16);
        if (missProbability == 1)
        {
            moveValue = 0;
            miss = true;
            return;
        }

        if (caster is Player)
        {
            int criticalMultiplier = 4;
            float gutsChance = caster.Luck / 500f;
            bool useGutsAsProbability = gutsChance > 1f / 20f;

            if (useGutsAsProbability)
            {
                if (Random.Range(0, 500) <= gutsChance)
                {
                    critical = true;
                    moveValue = criticalMultiplier * caster.Offense;
                    return;
                }
            }
            else
            {
                if (Random.Range(0, 20) <= 1)
                {
                    critical = true;
                    moveValue = criticalMultiplier * caster.Offense;
                    return;
                }
            }
            
        }
        moveValue = 2 * caster.Offense;

        var damagePercentageModifier = 0.25f;
        moveValue += Mathf.Abs((int)Mathf.Round(moveValue * Random.Range(-damagePercentageModifier, damagePercentageModifier)));
        moveValue = Mathf.Abs(moveValue);
    }
}
