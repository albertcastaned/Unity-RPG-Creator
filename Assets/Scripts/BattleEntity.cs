using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleEntity : MonoBehaviour
{
    private const string SHADER_COLOR_NAME = "_Color";
    protected Material material;
    private float colorAux;

    protected bool selected;

    private string myName;

    #region STATS

    protected int HP { get; set; }

    protected int MaxHP { get; set; }

    public int SP { get; set; }

    
    protected int MaxSP { get; set; }

    protected int Offense { get; set; }
    protected int Defense { get; set; }

    public int Speed { get; protected set; }

    #endregion

    #region STATUS
    public List<Status> statuses;
    
    public bool alive = true;
    public bool Alive { get => alive;
        protected set => alive = value; }
    #endregion

    protected Sprite sprite;

    protected SpriteRenderer spriteRenderer;

    public HealthBarController HealthBar;
    public HealthBarController SpecialBar;

    public List<SpecialData> specialMoves;

    public Inventory inventory;

    public GameObject itemUsedPrefab;

    private BattleEntity target;

    private List<BattleEntity> multipleTargets = new List<BattleEntity>();

    protected Animator animator;

    protected Vector3 originalPosition;


    public GameObject statusIconsHolder;

    public GameObject statusIcon;

    private SpecialData currentSpecial;

    private ItemData currentItem;

    private int turnsAsleep;
    private int turnsBurned;

    protected static BattleController battleController;
    
    private static readonly int StartAttack = Animator.StringToHash("Start Attack");

    private static readonly int AttackPosition = Animator.StringToHash("Attack Position");
    private static readonly int CastPosition = Animator.StringToHash("Cast Position");
    private static readonly int OriginalPosition = Animator.StringToHash("Returned to Original Position");
    private static readonly int AttackFinished = Animator.StringToHash("Attack Finished");

    private static readonly int Color = Shader.PropertyToID(SHADER_COLOR_NAME);


    public void SetSpecialAttack(SpecialData special)
    {
        currentSpecial = special;
    }

    public void SetItem(ItemData item)
    {
        currentItem = item;
    }
    public void SetTarget(BattleEntity targ)
    {
        this.target = targ;
    }

    public void SetMultipleTargets(List<BattleEntity> targets)
    {
        multipleTargets = targets;
    }
    public void StartMeleeAttack()
    {
        animator.SetTrigger(StartAttack);
        StartCoroutine(MoveTowardsAttackPoint());
    }

    public void StartSpecialAttack()
    {
        animator.SetTrigger(StartAttack);
        StartCoroutine(MoveTowardsSpecialPoint());
    }

    public void AddStatus(Status stat)
    {

        foreach (Transform slot in statusIconsHolder.transform)
        {
            if (slot.gameObject.name == stat.statusName)
            {
                return;
            }
        }
        statuses.Add(stat);
        AddStatusIcon(stat);
    }
    private void RemoveStatus(int index)
    {
        RemoveStatusIcon(statuses[index].statusName);
        statuses.RemoveAt(index);
    }
    private void AddStatusIcon(Status stat)
    {
        GameObject statusIconObj = Instantiate(statusIcon, statusIconsHolder.transform);

        statusIconObj.name = stat.statusName;

        statusIconObj.GetComponent<SpriteRenderer>().sprite = stat.icon;

    }

    private void RemoveStatusIcon(string stat)
    {
        foreach (Transform slot in statusIconsHolder.transform)
        {
            if (slot.gameObject.name != stat) continue;
            Destroy(slot.gameObject);
            break;
        }
    }
    public void ProcessStatus()
    { 
        if(!alive)
            return;
        for(int i = statuses.Count - 1; i > -1; i--)
        {
            switch (statuses[i].statusName)
            {

                case "Burned":
                    if (UnityEngine.Random.Range(1, 100) < (5 + turnsBurned * 5))
                    {
                        RemoveStatus(i);
                        turnsBurned = 0;
                    }
                    else
                    {
                        turnsBurned++;
                        ReceiveDamage(Mathf.Max((MaxHP / 3),2));
                    }
                    break;
                
                case "Defending":
                    RemoveStatus(i);
                    break;
            }
        }
        
    }
    public bool CanActInTurn()
    {
        if (!alive)
            return false;

        
        for(int i = statuses.Count - 1; i > -1; i--)
        {
            switch (statuses[i].statusName)
            {
                case "Asleep":
                    //Wake up
                    if (UnityEngine.Random.Range(1, 100) < (25 + turnsAsleep * 10))
                    {
                        RemoveStatus(i);
                        turnsAsleep = 0;
                        return true;
                    }
                    else
                    {
                        turnsAsleep++;
                        return false;
                    }
            }
        }


        
        return true;

    }
    private IEnumerator MoveTowardsAttackPoint()
    {
        if (target == null && multipleTargets == null)
        {
            Debug.Log("Target does not exist!");
            yield break;
        }

        float offset = (target is Player) ? 3f : -3f;
        Vector3 targetPoint = new Vector3(target.transform.position.x + offset, transform.position.y, target.transform.position.z);
        while(!V3Equal(transform.position, targetPoint))
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, 20f * Time.deltaTime);
            yield return null;
        }
        animator.SetTrigger(AttackPosition);

    }

    private IEnumerator MoveTowardsSpecialPoint()
    {
        if (target == null && multipleTargets == null)
        {
            Debug.Log("Target does not exist!");
            yield break;
        }

        float offset = (this is Player) ? 6f : -6f;
        Vector3 targetPoint = new Vector3(transform.position.x + offset, transform.position.y, 0f);
        while (!V3Equal(transform.position, targetPoint))
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, 18f * Time.deltaTime);
            yield return null;
        }
        animator.SetTrigger(CastPosition);

    }




    protected IEnumerator StartAction()
    {
        if (target == null && multipleTargets == null)
        {
            Debug.Log("Target does not exist!");
            yield break;
        }

        if (currentSpecial == null)
        {
            if (currentItem is null)
                //Regular Melee Attack
                target.ReceiveDamage(Offense);
            else
            {
                //Item used

                GameObject itemEffect = Instantiate(itemUsedPrefab);
                itemEffect.GetComponentInChildren<SpriteRenderer>().sprite = currentItem.image;
                itemEffect.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);

                if (currentItem.isAHeal)
                {
                    if (multipleTargets.Count > 0)
                    {
                        foreach (BattleEntity entity in multipleTargets)
                            entity.Heal(currentItem.value);
                    }
                    else
                    {
                        target.Heal(currentItem.value);

                    }

                    yield return new WaitForSeconds(1);
                    inventory.RemoveItem(currentItem);
                    ReturnToOriginalPosition();
                }
                else
                {

                    if (multipleTargets.Count > 0)
                    {
                        foreach (BattleEntity entity in multipleTargets)
                            entity.ReceiveDamage(currentItem.value);
                    }
                    else
                    {
                        target.ReceiveDamage(currentItem.value);

                    }
                    yield return new WaitForSeconds(1);
                    inventory.RemoveItem(currentItem);
                    ReturnToOriginalPosition();
                    

                }

                CheckItemsLeft();

            }
        }
        else
        {

            //Special Move
            ChangeSP(-currentSpecial.SPcost);

            if (currentSpecial.isAHeal)
            {
                if (multipleTargets.Count > 0)
                {
                    foreach (BattleEntity entity in multipleTargets)
                        entity.Heal(currentSpecial.value);
                }
                else
                {
                    target.Heal(currentSpecial.value);

                }
                ReturnToOriginalPosition();
            }
            else
            {
                if (currentSpecial.specialMoveAnimation == null)
                {
                    if (multipleTargets.Count > 0)
                    {
                        foreach (BattleEntity entity in multipleTargets)
                        {
                            entity.ReceiveDamage(currentSpecial.value);
                            if (currentSpecial.statusEffect.status != null)
                            {
                                if(UnityEngine.Random.Range(0,100) <= currentSpecial.statusEffect.chanceOfSuccess)
                                    entity.AddStatus(currentSpecial.statusEffect.status);
                                else if (currentSpecial.value <= 0)
                                    Instantiate(BattleController.instance.missPrefab, new Vector3(entity.transform.position.x, entity.transform.position.y + 1.5f, entity.transform.position.z), Quaternion.identity, battleController.worldCanvas);

                            }
                        }
                    }
                    else
                    {
                        target.ReceiveDamage(currentSpecial.value);
                        if (currentSpecial.statusEffect.status != null)
                        {
                            if (UnityEngine.Random.Range(0, 100) <= currentSpecial.statusEffect.chanceOfSuccess)
                                target.AddStatus(currentSpecial.statusEffect.status);
                            else if (currentSpecial.value <= 0)
                                Instantiate(BattleController.instance.missPrefab, new Vector3(target.transform.position.x, target.transform.position.y + 1.5f, target.transform.position.z), Quaternion.identity, battleController.worldCanvas);

                        }

                    }
                    ReturnToOriginalPosition();
                }
                else
                {
                    GameObject attackAnimation = Instantiate(currentSpecial.specialMoveAnimation);
                    AttackAnimationController attack = attackAnimation.GetComponent<AttackAnimationController>();
                    List<BattleEntity> targets = new List<BattleEntity>();

                    if (multipleTargets.Count > 0)
                    {
                        attack.SetTarget(multipleTargets);
                    }
                    else
                    {
                        targets.Add(target);
                        attack.SetTarget(targets);

                    }
                    attack.SetDamage(currentSpecial.value);
                    if (currentSpecial.statusEffect.status != null)
                    {
                        attack.SetStatus(currentSpecial.statusEffect);
                    }

                    while (attackAnimation != null)
                    {
                        yield return null;
                    }


                    ReturnToOriginalPosition();


                }


            }




        }
        target = null;

    }

    private void ClearTurn()
    {
        multipleTargets.Clear();
        currentSpecial = null;
        currentItem = null;

    }

    private IEnumerator MoveTowardsOriginalPosition()
    {
        ClearTurn();
        while (!V3Equal(transform.position, originalPosition))
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, 20f * Time.deltaTime);
            yield return null;
        }
        animator.SetTrigger(OriginalPosition);

        BattleController.instance.NextEntityTurn();
        
    }


    private void ReturnToOriginalPosition()
    {
        animator.SetTrigger(AttackFinished);
        StartCoroutine(MoveTowardsOriginalPosition());
    }
    public abstract void ReceiveDamage(int dmg);

    protected abstract void Heal(int amount);

    protected abstract void ChangeSP(int amount);

    protected abstract IEnumerator StartDeathSequence();
    protected abstract void CheckItemsLeft();
    public abstract void Revive(int healthRestore);
    

    protected void SelectedColorCycle()
    {
        if (selected)
        {
            colorAux += 8f * Time.deltaTime;

            SetColor(new Color(2f + Mathf.Sin(colorAux) / 4f, 2f + Mathf.Sin(colorAux) / 4f, 2f + Mathf.Sin(colorAux) / 4f, 1f));
        }
            
    }

    private void ResetColor()
    {
        SetColor(new Color(1f, 1f, 1f, 1f));
    }
    protected IEnumerator DamageShake(float duration, float magnitude)
    {
        if (HP <= 0)
        {
            yield return StartDeathSequence();
        }
        
        Transform myTransform = transform;

        Vector3 ogPos = myTransform.position;

        var elapsed = 0.0f;

        var originalScale = myTransform.localScale;
        while (elapsed < duration)
        {
            var x = originalPosition.x + UnityEngine.Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, originalPosition.y, originalPosition.z);
            transform.localScale = new Vector3(originalScale.x + UnityEngine.Random.Range(-0.2f, 0.2f), originalScale.y + UnityEngine.Random.Range(-0.1f, 0.1f), originalScale.z);
            elapsed += Time.deltaTime / 2f;
            yield return null;
        }

        myTransform.position = ogPos;
        myTransform.localScale = originalScale;



    }
    public void SetSelected(bool flag)
    {
        selected = flag;

        if (!selected)
            ResetColor();
    }
    protected void SetColor(Color color)
    {
        material.SetColor(Color, color);
    }

    private static bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.05;
    }


}
