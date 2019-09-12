using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleEntity : MonoBehaviour
{

    protected string myName;
    public string Name => myName;

    #region STATS
    private int hp;
    private int sp;
    private int offense;
    private int defense;
    private int speed;
    private int luck;

    private int hypnosisChance;
    private int paralysisChance;
    public int HP { get => hp; set => hp = value; }
    public int SP { get => sp; set => sp = value; }
    public int Offense { get => offense; set => offense = value; }
    public int Defense { get => defense; set => defense = value; }
    public int Speed { get => speed; set => speed = value; }
    public int Luck { get => luck; set => luck = value; }
    public int HypnosisChance { get => hypnosisChance; set => hypnosisChance = value; }
    public int ParalysisChance { get => paralysisChance; set => paralysisChance = value; }
    #endregion

    #region STATUS
    private EntityStatus status = EntityStatus.None;
    public EntityStatus Status { get => status; set => status = value; }

    private bool defending;
    public bool Defending { get => defending; set => defending = value; }

    private bool alive = true;
    public bool Alive { get => alive; set => alive = value; }
    #endregion

    protected Sprite sprite;

    public List<SpecialData> specialMoves;
    public List<ItemData> items;
    //TODO Agregar items 

    public abstract void ReceiveDamage(int dmg);

    public abstract void Heal(int amount);

    public abstract void ChangeSP(int amount);

    public abstract void StartDeathSequence();

    public abstract void Revive(int healthRestore);

    public static explicit operator BattleEntity(List<Enemy> v)
    {
        throw new NotImplementedException();
    }
}
