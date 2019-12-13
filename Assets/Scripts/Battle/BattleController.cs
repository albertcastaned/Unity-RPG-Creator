using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

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

    private int entityIndex;
    private int playerIndex;
    private int enemySelectIndex;
    private int partySelectorIndex;
    private int partyMultiTargetIndex;
    private int commandIndex;

    public List<Enemy> currentEnemies;
    public List<Player> currentPlayers;

    public List<BattleEntity> battleEntities; 


    public EntitySelector entitySelector;

    public MenuController descriptionController;


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

    private void InitiateBattleEntities()
    {
        foreach (Player player in currentPlayers)
        {
            battleEntities.Add(player);

        }
        foreach (Enemy enemy in currentEnemies)
        {
            battleEntities.Add(enemy);
            print(enemy.Speed);
        }

        battleEntities = new List<BattleEntity>(battleEntities.OrderByDescending(entity => entity.Speed));
        

    }

    void Start()
    {
        currentCommands = new List<Command>();
        InitiateBattleEntities();
        currentBattleState = BattleState.PlayerTurn;

        if (battleEntities[entityIndex] is Enemy)
            currentBattleState = BattleState.EnemyTurn;
        else if (battleEntities[entityIndex] is Player)
        {

            currentBattleState = BattleState.PlayerTurn;
            descriptionController.ACTIVE = true;
            ((Player)battleEntities[entityIndex]).ActivateMenu();

        }
    }

    public void NextEntityTurn()
    {
        entityIndex++;
        while (entityIndex < battleEntities.Count - 1 && !battleEntities[entityIndex].Alive)
        {
            entityIndex++;
        }

        if (entityIndex > battleEntities.Count - 1)
            entityIndex = 0;

        if (battleEntities[entityIndex] is Enemy)
            currentBattleState = BattleState.EnemyTurn;
        else if (battleEntities[entityIndex] is Player)
        {

            currentBattleState = BattleState.PlayerTurn;
            descriptionController.ACTIVE = true;
            ((Player)battleEntities[entityIndex]).ActivateMenu();

        }

        StartCoroutine(PauseForSeconds(0.5f));
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
        switch (currentBattleState)
        {

            case BattleState.PlayerTurn:
                {
                    switch (currentMenuState)
                    {
                        case MenuState.Idle:
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                switch (((Player)battleEntities[entityIndex]).GetSelection())
                                {
                                    case TypeOfCommand.Melee:
                                        enemySelectIndex = 0;
                                        currentMenuState = MenuState.SelectingEnemyMelee;
                                        entitySelector.gameObject.SetActive(true);
                                        ((Player)battleEntities[entityIndex]).DeactivateMenu();
                                        SetSelectorToEnemy(currentEnemies[enemySelectIndex]);

                                        break;

                                }
                            }
                            break;
                        case MenuState.SelectingEnemyMelee:
                            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)) && enemySelectIndex > 0)
                            {
                                currentEnemies[enemySelectIndex].SetSelected(false);

                                enemySelectIndex--;
                                SetSelectorToEnemy(currentEnemies[enemySelectIndex]);
                            }

                            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W)) && enemySelectIndex < currentEnemies.Count - 1)
                            {
                                currentEnemies[enemySelectIndex].SetSelected(false);

                                enemySelectIndex++;
                                SetSelectorToEnemy(currentEnemies[enemySelectIndex]);

                            }
                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                currentMenuState = MenuState.Idle;
                                entitySelector.gameObject.SetActive(false);
                                currentEnemies[enemySelectIndex].SetSelected(false);
                                ((Player)battleEntities[entityIndex]).ActivateMenu();
                            }

                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                currentMenuState = MenuState.Idle;
                                entitySelector.gameObject.SetActive(false);
                                currentEnemies[enemySelectIndex].SetSelected(false);

                                battleEntities[entityIndex].SetTarget(currentEnemies[enemySelectIndex]);
                                battleEntities[entityIndex].StartAttack();
                                descriptionController.ACTIVE = false;
                                PAUSE = true;

                            }


                            break;
                    }

                    break;

                }
            case BattleState.EnemyTurn:
                {
                    if (!battleEntities[entityIndex].Alive)
                    {
                        NextEntityTurn();
                        break;
                    }
                    battleEntities[entityIndex].SetTarget(currentPlayers[Random.Range(0, currentPlayers.Count)]);
                    battleEntities[entityIndex].StartAttack();
                    PAUSE = true;
                    break;
                }
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

    private void SetSelectorToEnemy(Enemy enemy)
    {
        Vector3 dest = new Vector3(enemy.gameObject.transform.position.x, enemy.gameObject.transform.position.y + 5f, enemy.gameObject.transform.position.z);
        entitySelector.SetDestination(dest);

        enemy.SetSelected(true);
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
