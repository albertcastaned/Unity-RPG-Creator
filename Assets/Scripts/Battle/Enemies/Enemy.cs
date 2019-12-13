using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class Enemy : BattleEntity
{
    public EnemyData enemyData;

    public GameObject damageUIPrefab;
    private string description;

    private List<SpecialData> moves;

    private List<ItemDrops> drops;

    private int expDrop;

    public string Description { get => description; set => description = value; }
    public List<SpecialData> Moves { get => moves; set => moves = value; }
    public List<ItemDrops> Drops { get => drops; set => drops = value; }
    public int ExpDrop { get => expDrop; set => expDrop = value; }
    public override void ChangeSP(int amount)
    {
        SP -= amount;
    }
    void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        originalPosition = transform.position;
        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = enemyData.animator;

        //animator = enemyData.animator;

        HP = enemyData.HP;
        SP = enemyData.SP;
        MaxHP = enemyData.HP;
        MaxSP = enemyData.SP;
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

    void Start()
    {
        if (HealthBar != null)
        {
            HealthBar.ChangeValue(HP, MaxHP);
        }
        if (SpecialBar != null)
        {
            SpecialBar.ChangeValue(SP, MaxSP);
        }
    }
    void Update()
    {
        if (!Alive)
            return;
        SelectedColorCycle();
    }
    public override void Heal(int amount)
    {
        HP += amount;
    }

    public override void ReceiveDamage(int dmg)
    {
        StartCoroutine(DamageShake(0.4f, 0.2f));

        int total = dmg - defense;
        HP -= Mathf.Max(0, total);
        print("Received " + total + " of damage");
        GameObject aux = Instantiate(damageUIPrefab, GameObject.Find("WorldSpaceCanvas").transform);

        aux.GetComponent<TextMeshProUGUI>().text = total.ToString();
        aux.transform.position = new Vector3(transform.position.x, transform.position.y + 200f, transform.position.z);


        if (HealthBar != null)
        {
            HealthBar.ChangeValue(HP, MaxHP);
        }
    }

    public override void Revive(int healthRestore)
    {
        Alive = true;
        HP += healthRestore;
    }

    public override IEnumerator StartDeathSequence()
    {
        print("Started coroutine");
        Alive = false;

        float opacity = 1f;
        while(opacity > 0.1f)
        {
            opacity -= 0.05f;
            SetColor(new Color(1f, 1f, 1f, opacity));
            yield return null;
        }
        BattleController.instance.RemoveEnemy(this);
        Destroy(gameObject);
    }

    public void ChooseAttack()
    {

    }

}
