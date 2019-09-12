using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class BattleController : MonoBehaviour
{

    //Singleton instance
    public static BattleController instance;

    //Battle States
    #region States
    private enum BattleState
    {
        PlayerTurn,
        EnemyTurn,
        Commands,
        Won,
        Lose,
        Flee
    }

    private enum MenuState
    {
        Idle,
        ChoosingSpecialMove,
        SelectingEnemyMelee,
        SelectingEnemySpecial,
        ChoosingItem,
        ChoosingPartyItem,
        ChoosingPartySpecial,
        TurnOver
    }

    private MoveDirection moveDirection;

    #endregion

    private BattleState currentBattleState;
    private MenuState currentMenuState;

    private List<Command> currentCommands;

    private int playerIndex;
    private int enemySelectIndex;
    private int partySelectorIndex;
    private int partyMultiTargetIndex;
    private int commandIndex;

    public List<Enemy> currentEnemies;
    public List<Player> currentPlayers;

    public GameObject actionMenu;


    private bool PAUSE;

    private void BattleAction()
    {
        Command thisCommand = currentCommands[commandIndex];

        foreach(BattleEntity entity in thisCommand.Targets)
        {
            switch(currentCommands[commandIndex].CommandType)
            {
                case TypeOfCommand.Melee:
                    entity.ReceiveDamage(thisCommand.Value);
                    print("Received damage");
                    break;
                case TypeOfCommand.Item:

                    break;
                case TypeOfCommand.Special:

                    break;
            }
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        currentCommands = new List<Command>();
        currentBattleState = BattleState.PlayerTurn;
    }

    // Update is called once per frame
    void Update()
    {
        if (PAUSE)
            return;

        if(currentEnemies.Count == 0 && currentBattleState != BattleState.Won)
        {
            currentCommands.Clear();
            currentBattleState = BattleState.Won;
            //TODO Result Screen
            print("WON");
        }
        switch(currentBattleState)
        {
            case BattleState.Commands:
                while (commandIndex < currentCommands.Count && !currentCommands[commandIndex].Caster.Alive)
                    commandIndex++;
                if (commandIndex >= currentCommands.Count)
                {
                    currentBattleState = BattleState.PlayerTurn;
                    currentCommands.Clear();
                    commandIndex = 0;
                    playerIndex = 0;

                    while (!currentPlayers[playerIndex].Alive)
                        playerIndex++;
                }
                else
                {
                    BattleAction();
                    commandIndex++;
                }
                break;
            case BattleState.PlayerTurn:
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    List<BattleEntity> targets = new List<BattleEntity>();
                    targets.Add(currentEnemies[0]);

                    currentCommands.Add(new Command(currentPlayers[playerIndex], targets));
                    currentBattleState = BattleState.EnemyTurn;
                }
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    List<BattleEntity> targets = new List<BattleEntity>();
                    foreach(Enemy enemy in currentEnemies)
                    {
                        targets.Add(enemy);
                    }
                    currentCommands.Add(new Command(currentPlayers[playerIndex], targets));
                    currentBattleState = BattleState.EnemyTurn;
                }
                break;
            case BattleState.EnemyTurn:

                foreach(Enemy enemy in currentEnemies)
                {
                    List<BattleEntity> targets = new List<BattleEntity>
                    {
                        currentPlayers[0]
                    };
                    currentCommands.Add(new Command(enemy, targets));

                }
                currentBattleState = BattleState.Commands;
                break;
            case BattleState.Won:

                break;
            case BattleState.Lose:
                print("LOST");
                break;
        }
    }

    private IEnumerator PauseForSeconds(float duration)
    {
        PAUSE = true;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {


            elapsed += Time.deltaTime;
            yield return null;
        }
        PAUSE = false;
    }
    private static bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.05;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        currentEnemies.Remove(enemy);
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), currentBattleState.ToString());

        int i = 0;
        foreach(BattleEntity player in currentPlayers)
        {
            GUI.Label(new Rect(10, 40 + (30 * i), 100, 20), currentPlayers[i].HP.ToString());
            i++;
        }
        i = 0;

        foreach (BattleEntity enemy in currentEnemies)
        {
            GUI.Label(new Rect(10, 300 + (30 * i), 100, 20), currentEnemies[i].HP.ToString());
        }
    }
}
