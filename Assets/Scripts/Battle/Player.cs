using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

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
    public GameObject damageUIPrefab;

    public CommandSelectController playerMenuController;

    void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
        HP = MyHP;
        MaxHP = MyMaxHP;
        SP = MySP;
        MaxSP = MyMaxSP;
        Offense = MyOffense;
        Defense = MyDefense;
        Luck = MyLuck;
        Speed = MySpeed;

    }

    void Start()
    {
        if (HealthBar != null)
        {
            HealthBar.ChangeValue(HP, MaxHP);
        }
        if (HealthBar != null)
        {
            SpecialBar.ChangeValue(SP, MaxSP);
        }
    }

    public void ActivateMenu()
    {
        playerMenuController.ACTIVE = true;
    }

    public void DeactivateMenu()
    {
        playerMenuController.ACTIVE = false;
    }
    public TypeOfCommand GetSelection()
    {
        return playerMenuController.GetSelection();
    }
    public override void ChangeSP(int amount)
    {
        SP -= amount;
    }
    void Update()
    {
        SelectedColorCycle();
    }
    public override void Heal(int amount)
    {
        HP += amount;
    }

    public override void ReceiveDamage(int dmg)
    {
        int total = dmg - defense;
        HP -= Mathf.Max(0, total);
        print("Received " + total + " of damage, has " + HP);
        GameObject aux = Instantiate(damageUIPrefab, GameObject.Find("WorldSpaceCanvas").transform);

        aux.GetComponent<TextMeshProUGUI>().text = total.ToString();
        aux.transform.position = new Vector3(transform.position.x, transform.position.y + 200f, transform.position.z);

        StartCoroutine(DamageShake(0.3f, 0.05f));
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
        yield break;
    }

}
