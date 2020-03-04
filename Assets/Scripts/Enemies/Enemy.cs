using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

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

    protected override void ChangeSP(int amount)
    {
        SP -= amount;
    }
    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        originalPosition = transform.position;
        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = enemyData.animator;

        //animator = enemyData.animator;

        HP = enemyData.HP;
        SP = enemyData.SP;
        MaxHP = enemyData.HP;
        MaxSP = enemyData.SP;
        Offense = enemyData.Offense;
        Speed = enemyData.Speed;
        sprite = enemyData.sprite;
        Moves = enemyData.specialMoves;
        Description = enemyData.description;
        Drops = enemyData.itemsDrop;
        ExpDrop = enemyData.ExpDrop;


    }

    void Start()
    {
        battleController = BattleController.instance;
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

        if (selected)
            SelectedColorCycle();
    }

    protected override void Heal(int amount)
    {
        HP += amount;

    }

    public override void ReceiveDamage(int dmg)
    {
        if (dmg == 0)
            return;

        StartCoroutine(DamageShake(0.1f, 0.1f));


        int total = Mathf.Max(Mathf.Abs(dmg) - Defense, 0);
        HP = Mathf.Max(0, HP - total);
        
        if(HP <= 0)
        {
            Alive = false;
            BattleController.instance.RemoveEnemy(this);
            StartCoroutine(StartDeathSequence());
        }

        GameObject aux = Instantiate(battleController.damageUIPrefab, new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.identity, battleController.worldCanvas);

        aux.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = total.ToString();
        if (HealthBar != null)
        {
            HealthBar.ChangeValue(HP, MaxHP);
        }
    }

    protected override void CheckItemsLeft()
    {
        throw new System.NotImplementedException();
    }

    public override void Revive(int healthRestore)
    {
        Alive = true;
        HP += healthRestore;
    }

    protected override IEnumerator StartDeathSequence()
    {

        
        float opacity = 1f;
        while(opacity > 0.1f)
        {
            opacity -= 0.05f;
            SetColor(new Color(1f, 1f, 1f, opacity));
            yield return null;
        }
        Destroy(gameObject);
    }

    public void ChooseAttack()
    {

    }

}
