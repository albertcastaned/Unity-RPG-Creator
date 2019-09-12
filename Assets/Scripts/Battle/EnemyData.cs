using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]

public class EnemyData : ScriptableObject
{
    [Header("Description")]
    public string enemyName = "Enemy";
    public Sprite sprite;
    public string description = "A hostile enemy.";

    [Header("Stats")]
    public int HP = 10;
    public int SP = 10;
    public int Offense = 10;
    public int Defense = 10;
    public int Speed = 10;
    public int Luck = 10;
    [Range(0,100)]
    public int HypnosisChance = 10;
    [Range(0, 100)]
    public int ParalysisChance = 10;


    [Header("Special Moves")]
    public List<SpecialData> specialMoves;

    [Header("Exp & Item Drop")]
    public int ExpDrop = 1;
    public List<ItemDrops> itemsDrop;


}
