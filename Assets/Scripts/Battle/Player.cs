public class Player : BattleEntity
{

    public int MyHP;
    public int MySP;
    public int MyOffense;
    public int MyDefense;
    public int MyLuck;
    public int MySpeed;

    void Start()
    {
        HP = MyHP;
        SP = MySP;
        Offense = MyOffense;
        Defense = MyDefense;
        Luck = MyLuck;
        Speed = MySpeed;
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
    }

    public override void Revive(int healthRestore)
    {
        Alive = true;
        HP += healthRestore;
    }

    public override void StartDeathSequence()
    {
        Alive = false;
    }

}
