using UnityEngine;
using System.Collections.Generic;
public class Enemy : BattleEntity
{
    public EnemyData enemyData;
    private string description;

    private List<SpecialData> moves;

    private List<ItemDrops> drops;

    private int expDrop;

    public string Description { get => description; set => description = value; }
    public List<SpecialData> Moves { get => moves; set => moves = value; }
    public List<ItemDrops> Drops { get => drops; set => drops = value; }
    public int ExpDrop { get => expDrop; set => expDrop = value; }

    void Start()
    {
        HP = enemyData.HP;
        SP = enemyData.SP;
        Offense = enemyData.Offense;
        Defense = enemyData.Defense;
        Speed = enemyData.Speed;
        Luck = enemyData.Luck;
        HypnosisChance = enemyData.HypnosisChance;
        ParalysisChance = enemyData.ParalysisChance;
        sprite = enemyData.sprite;
        Moves = enemyData.specialMoves;
        Description = enemyData.description;
        Drops = enemyData.itemsDrop;
        ExpDrop = enemyData.ExpDrop;
    }
    public override void ChangeSP(int amount)
    {
        SP -= amount;
    }

    public override void Heal(int amount)
    {
        HP += amount;
    }

    public override void ReceiveDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0)
            StartDeathSequence();
    }

    public override void Revive(int healthRestore)
    {
        Alive = true;
        HP += healthRestore;
    }

    public override void StartDeathSequence()
    {
        Alive = false;
        BattleController.instance.RemoveEnemy(this);
        Destroy(gameObject);
    }

    public void ChooseAttack()
    {

    }

}
