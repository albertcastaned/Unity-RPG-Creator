using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleEntity : MonoBehaviour
{
    protected const string SHADER_COLOR_NAME = "_Color";
    protected Material material;
    protected float colorAux = 0f;

    protected bool selected;

    protected string myName;
    public string Name => myName;

    #region STATS
    protected int hp;
    protected int maxHP;
    protected int sp;
    protected int maxSP;
    protected int offense;
    protected int defense;
    protected int speed;
    protected int luck;

    protected int hypnosisChance;
    protected int paralysisChance;
    public int HP { get => hp; set => hp = value; }
    public int MaxHP { get => maxHP; set => maxHP = value; }

    public int SP { get => sp; set => sp = value; }
    public int MaxSP { get => maxSP; set => maxSP = value; }

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

    public HealthBarController HealthBar;
    public HealthBarController SpecialBar;

    public List<SpecialData> specialMoves;
    public List<ItemData> items;



    protected BattleEntity meleeTarget;


    protected Animator animator;

    protected Vector3 originalPosition;

    
    //TODO Agregar items 


    public void SetTarget(BattleEntity target)
    {
        meleeTarget = target;
    }
    public void StartAttack()
    {
        animator.SetTrigger("Start Attack");
        StartCoroutine(MoveTowardsAttackPoint());
    }

    private IEnumerator MoveTowardsAttackPoint()
    {
        if (meleeTarget == null)
        {
            Debug.Log("Target does not exist!");
            yield break;
        }

        float offset = (meleeTarget is Player) ? 3f : -3f;
        Vector3 targetPoint = new Vector3(meleeTarget.transform.position.x + offset, meleeTarget.transform.position.y, meleeTarget.transform.position.z);
        while(!V3Equal(transform.position, targetPoint))
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, 20f * Time.deltaTime);
            yield return null;
        }
        animator.SetTrigger("Attack Position");

    }
    protected void InflictDamageToTarget()
    {
        if (meleeTarget == null)
        {
            Debug.Log("Target does not exist!");
            return;
        }

        meleeTarget.ReceiveDamage(offense);

        meleeTarget = null;

    }
    protected IEnumerator MoveTowardsOriginalPosition()
    {

        while (!V3Equal(transform.position, originalPosition))
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, 20f * Time.deltaTime);
            yield return null;
        }
        animator.SetTrigger("Returned to Original Position");

        BattleController.instance.NextEntityTurn();


    }


    protected void ReturnToOriginalPosition()
    {
        animator.SetTrigger("Attack Finished");
        StartCoroutine(MoveTowardsOriginalPosition());
    }
    public abstract void ReceiveDamage(int dmg);

    public abstract void Heal(int amount);

    public abstract void ChangeSP(int amount);

    public abstract IEnumerator StartDeathSequence();

    public abstract void Revive(int healthRestore);

    public static explicit operator BattleEntity(List<Enemy> v)
    {
        throw new NotImplementedException();
    }

    public void SelectedColorCycle()
    {
        if (selected)
        {
            colorAux += 8f * Time.deltaTime;

            SetColor(new Color(2f + Mathf.Sin(colorAux) / 4f, 2f + Mathf.Sin(colorAux) / 4f, 2f + Mathf.Sin(colorAux) / 4f, 1f));
        }
        else
            SetColor(new Color(1f, 1f, 1f, 1f));
    }

    protected IEnumerator DamageShake(float duration, float magnitude)
    {

        var originalPosition = transform.position;

        var elapsed = 0.0f;
        while (elapsed < duration)
        {
            var x = originalPosition.x + UnityEngine.Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, originalPosition.y, originalPosition.z);

            elapsed += Time.deltaTime / 2f;
            yield return null;
        }
        transform.position = originalPosition;


        if (HP <= 0)
            StartCoroutine(StartDeathSequence());
    }
    public void SetSelected(bool flag)
    {
        selected = flag;
    }
    protected void SetColor(Color color)
    {
        material.SetColor(SHADER_COLOR_NAME, color);
    }

    private static bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.05;
    }

}
