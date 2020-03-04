using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using System.Linq;

public class Player : BattleEntity
{
    public int MyHP;
    public int MyMaxHP;
    public int MySP;
    public int MyMaxSP;
    public int MyOffense;
    public int MyDefense;
    public int MyLuck;
    public int MySpeed;

    public CommandSelectController playerMenuController;

    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
        HP = MyHP;
        MaxHP = MyMaxHP;
        SP = MySP;
        MaxSP = MyMaxSP;
        Offense = MyOffense;
        Speed = MySpeed;
        Defense = MyDefense;

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

    public void DeactivateCommand(TypeOfCommand command)
    {
        int i = 0;
        foreach(PlayerMenuChoice choice in playerMenuController.playerChoices)
        {
            if (choice.typeOfCommand == command)
            {
                break;
            }

            i++;
        }

        if (i <= playerMenuController.GetSlots().Count - 1)
        {
            playerMenuController.GetSlots()[i].SetToDisabled();

        }
    }
    
    public void ActivateMenu()
    {
        playerMenuController.ActivateMenu();
    }

    public void DeactivateMenu()
    {
        playerMenuController.DeactivateMenu();
    }
    public TypeOfCommand GetSelection()
    {
        return playerMenuController.GetSelection();
    }

    protected override void ChangeSP(int amount)
    {
        SP += amount;

        if (SP < 0)
            SP = 0;
        if (SP > MaxSP)
            SP = MaxSP;
        
        if (HealthBar != null)
        {
            SpecialBar.ChangeValue(SP, MaxSP);
        }
    }
    void Update()
    {
        if (selected)
            SelectedColorCycle();
    }

    protected override void Heal(int amount)
    {
        
        HP += amount;
        if (HP > MyMaxHP)
            HP = MyMaxHP;
        GameObject aux = Instantiate(battleController.healUIPrefab, new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.identity, battleController.worldCanvas);

        aux.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = amount.ToString();

        GameObject healParticles = Instantiate(battleController.healParticle);

        healParticles.GetComponent<HealEffectController>().Setup(transform);

        if (HealthBar != null)
        {
            HealthBar.ChangeValue(HP, MaxHP);

        }
    }

    private bool isDefending()
    {
        return statuses.Any(status => status.statusName == "Defending");
    }
    public override void ReceiveDamage(int dmg)
    {
        if (dmg == 0)
            return;
        

        dmg = Mathf.Abs(dmg / (isDefending() ? 2 : 1));
        int total = dmg - Defense;
        HP = Mathf.Max(0, HP - total);
        GameObject aux = Instantiate(battleController.damageUIPrefab, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity, battleController.worldCanvas);

        aux.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = total.ToString();

        StartCoroutine(DamageShake(0.1f, 0.1f));

        
        if (HealthBar != null)
        {
            HealthBar.ChangeValue(HP, MaxHP);
        }

    }

    protected override void CheckItemsLeft()
    {
        if (inventory.GetItemCount() == 0)
        {
            DeactivateCommand(TypeOfCommand.Item);
        }
    }

    public override void Revive(int healthRestore)
    {
        Alive = true;
        HP += healthRestore;
    }

    protected override IEnumerator StartDeathSequence()
    {

        SetColor(new Color(0.1f, 0.1f, 0.1f, 1f));
        yield break;
    }

}
