using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using Random = UnityEngine.Random;

public class BattleController : MonoBehaviour
{
    private bool DEBUG;
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
        ChoosingItem,
        SelectingSingleEnemy,
        SelectingAllEnemies,
        SelectingSingleAlly,
        SelectingAllAllies
    }

    private MenuState previousMenuState = MenuState.Idle;

    private MoveDirection moveDirection;

    #endregion

    private BattleState currentBattleState;
    private MenuState currentMenuState;


    private int entityIndex;
    private int playerIndex;
    private int enemySelectIndex;
    private int partySelectorIndex;
    private int partyMultiTargetIndex;

    public List<Enemy> currentEnemies;
    public List<Player> currentPlayers;

    public List<BattleEntity> battleEntities; 


    public EntitySelector entitySelector;


    public ActionMenuController actionMenuController;

    public GameObject missPrefab;
    public GameObject damageUIPrefab;
    public GameObject healUIPrefab;
    public GameObject healParticle;

    [HideInInspector]
    public Transform worldCanvas;
    private bool PAUSE;


    public Status defenseStatus;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        worldCanvas = GameObject.Find("WorldSpaceCanvas").transform;
    }

    private void InitiateBattleEntities()
    {
        foreach (Player player in currentPlayers)
        {
            battleEntities.Add(player);

            if (player.inventory.GetItemCount() == 0)
            {
                player.DeactivateCommand(TypeOfCommand.Item);
            }
            
            if (player.specialMoves.Count == 0)
            {
                player.DeactivateCommand(TypeOfCommand.Special);
            }

        }
        foreach (Enemy enemy in currentEnemies)
        {
            battleEntities.Add(enemy);

        }

        battleEntities = new List<BattleEntity>(battleEntities.OrderByDescending(entity => entity.Speed));
        

    }

    void Start()
    {
        InitiateBattleEntities();
        currentBattleState = BattleState.PlayerTurn;

        if (battleEntities[entityIndex] is Enemy)
            currentBattleState = BattleState.EnemyTurn;
        else if (battleEntities[entityIndex] is Player)
        {

            currentBattleState = BattleState.PlayerTurn;
            ((Player)battleEntities[entityIndex]).ActivateMenu();

        }
    }

    public void NextEntityTurn()
    {

        entityIndex++;

        if (entityIndex > battleEntities.Count - 1)
            entityIndex = 0;

        //Check dead or invalid status
        while (!battleEntities[entityIndex].CanActInTurn())
        {

            battleEntities[entityIndex].ProcessStatus();
            entityIndex++;
            if (entityIndex > battleEntities.Count - 1)
                entityIndex = 0;
        }

        if (entityIndex > battleEntities.Count - 1)
            entityIndex = 0;

        battleEntities[entityIndex].ProcessStatus();

        if (battleEntities[entityIndex] is Enemy)
            currentBattleState = BattleState.EnemyTurn;
        else if (battleEntities[entityIndex] is Player)
        {

            currentBattleState = BattleState.PlayerTurn;
            previousMenuState = MenuState.Idle;
            ((Player)battleEntities[entityIndex]).ActivateMenu();

        }

        StartCoroutine(PauseForSeconds(0.8f));
    }


    private List<string> GetSpecialMovesNames(List<SpecialData> specialMoves)
    {
        List<string> moves = new List<string>();
        foreach(SpecialData special in specialMoves)
        {
            moves.Add(special.moveName);
        }
        return moves;
    }

    private List<string> GetItemsNames(List<ItemData> itms)
    {
        List<string> items = new List<string>();
        foreach (ItemData item in itms)
        {
            items.Add(item.itemName);
        }
        return items;
    }
    void ActivateSingleEnemySelect()
    {
        enemySelectIndex = 0;
        currentMenuState = MenuState.SelectingSingleEnemy;
        entitySelector.gameObject.SetActive(true);
        SetSelectorToEntity(currentEnemies[enemySelectIndex]);
    }

    void ActivateSingleAllySelect()
    {
        partySelectorIndex = 0;
        currentMenuState = MenuState.SelectingSingleAlly;
        entitySelector.gameObject.SetActive(true);
        SetSelectorToEntity(currentPlayers[partySelectorIndex]);
    }
    void ActivateAllEnemiesSelect()
    {
        currentMenuState = MenuState.SelectingAllEnemies;

        foreach(Enemy enemy in currentEnemies)
        {
            enemy.SetSelected(true);
        }
    }

    void ActivateAllAlliesSelect()
    {
        currentMenuState = MenuState.SelectingAllAllies;

        foreach (Player player in currentPlayers)
        {
            player.SetSelected(true);
        }
    }
    void ReturnPreviousMenuState()
    {
        switch (previousMenuState)
        {
            case MenuState.Idle:
                ((Player)battleEntities[entityIndex]).ActivateMenu();
                break;

            case MenuState.ChoosingItem:
            case MenuState.ChoosingSpecialMove:
                actionMenuController.SetupActionMenu(true);
                break;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (PAUSE)
            return;


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
                                        ((Player)battleEntities[entityIndex]).DeactivateMenu();

                                        ActivateSingleEnemySelect();

                                        break;

                                    case TypeOfCommand.Special:
                                        if (((Player)battleEntities[entityIndex]).specialMoves.Count > 0)
                                        {
                                            currentMenuState = MenuState.ChoosingSpecialMove;
                                            ((Player)battleEntities[entityIndex]).DeactivateMenu();
                                            
                                            actionMenuController.isSpecialMoves = true;
                                            
                                            actionMenuController.SetActionNames(GetSpecialMovesNames(((Player)battleEntities[entityIndex]).specialMoves));
                                            actionMenuController.SetupActionMenu(true);
                                        }
                                        break;

                                    case TypeOfCommand.Item:
                                        if (((Player)battleEntities[entityIndex]).inventory.GetItemCount() > 0)
                                        {
                                            currentMenuState = MenuState.ChoosingItem;
                                            ((Player)battleEntities[entityIndex]).DeactivateMenu();
                                            actionMenuController.isSpecialMoves = false;

                                            actionMenuController.SetActionNames(GetItemsNames(((Player)battleEntities[entityIndex]).inventory.items));

                                            actionMenuController.SetupActionMenu(true);

                                        }
                                        break;
                                    case TypeOfCommand.Defend:
                                        battleEntities[entityIndex].AddStatus(defenseStatus);
                                        ((Player)battleEntities[entityIndex]).DeactivateMenu();
                                        NextEntityTurn();
                                        break;
                                    case TypeOfCommand.Check:
                                        break;
                                    case TypeOfCommand.Flee:
                                        break;

                                    default:
                                        currentMenuState = MenuState.Idle;
                                        break;
                                }
                            }
                            break;

                        case MenuState.ChoosingSpecialMove:
                            if (Input.GetKeyDown(KeyCode.Space) && CanUseSpecialMove(actionMenuController.GetSelection()))
                            {
                                actionMenuController.SetupActionMenu(false);
                                previousMenuState = MenuState.ChoosingSpecialMove;

                                switch(battleEntities[entityIndex].specialMoves[actionMenuController.GetSelection()].moveDirection)
                                {


                                    case MoveDirection.SingleTarget:
                                        ActivateSingleEnemySelect();
                                        break;
                                    case MoveDirection.SingleAlly:
                                        ActivateSingleAllySelect();
                                        break;

                                    case MoveDirection.Opponents:
                                        ActivateAllEnemiesSelect();
                                        break;

                                    case MoveDirection.Allies:
                                        ActivateAllAlliesSelect();
                                        break;
                                }
                                 


                            }
                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                actionMenuController.SetupActionMenu(false);
                                previousMenuState = MenuState.Idle;
                                currentMenuState = MenuState.Idle;
                                ((Player)battleEntities[entityIndex]).ActivateMenu();
                            }
                            break;

                        case MenuState.ChoosingItem:
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                actionMenuController.SetupActionMenu(false);
                                previousMenuState = MenuState.ChoosingItem;

                                switch (((Player)battleEntities[entityIndex]).inventory.GetItem(actionMenuController.GetSelection()).moveDirection)
                                {

                                    case MoveDirection.SingleTarget:
                                        ActivateSingleEnemySelect();
                                        break;
                                    case MoveDirection.SingleAlly:
                                        ActivateSingleAllySelect();
                                        break;

                                    case MoveDirection.Opponents:
                                        ActivateAllEnemiesSelect();
                                        break;

                                    case MoveDirection.Allies:
                                        ActivateAllAlliesSelect();
                                        break;
                                }



                            }
                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                actionMenuController.SetupActionMenu(false);
                                previousMenuState = MenuState.Idle;
                                currentMenuState = MenuState.Idle;
                                ((Player)battleEntities[entityIndex]).ActivateMenu();
                            }
                            break;
                        case MenuState.SelectingSingleEnemy:
                            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)) && enemySelectIndex > 0)
                            {
                                currentEnemies[enemySelectIndex].SetSelected(false);

                                enemySelectIndex--;
                                SetSelectorToEntity(currentEnemies[enemySelectIndex]);
                            }

                            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W)) && enemySelectIndex < currentEnemies.Count - 1)
                            {
                                currentEnemies[enemySelectIndex].SetSelected(false);

                                enemySelectIndex++;
                                SetSelectorToEntity(currentEnemies[enemySelectIndex]);

                            }
                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                currentMenuState = previousMenuState;
                                entitySelector.gameObject.SetActive(false);
                                currentEnemies[enemySelectIndex].SetSelected(false);
                                ReturnPreviousMenuState();
                            }

                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                currentMenuState = MenuState.Idle;
                                entitySelector.gameObject.SetActive(false);
                                currentEnemies[enemySelectIndex].SetSelected(false);

                                battleEntities[entityIndex].SetTarget(currentEnemies[enemySelectIndex]);
                                if (previousMenuState == MenuState.ChoosingSpecialMove)
                                {
                                    SpecialData special = battleEntities[entityIndex].specialMoves[actionMenuController.GetSelection()];
                                    battleEntities[entityIndex].SetSpecialAttack(special);
                                    battleEntities[entityIndex].StartSpecialAttack();
                                }
                                else if(previousMenuState == MenuState.ChoosingItem)
                                {
                                    ItemData item = ((Player)battleEntities[entityIndex]).inventory.GetItem(actionMenuController.GetSelection());

                                    battleEntities[entityIndex].SetItem(item);
                                    battleEntities[entityIndex].StartSpecialAttack();
                                }
                                else
                                    battleEntities[entityIndex].StartMeleeAttack();
                                PAUSE = true;

                            }


                            break;
                        case MenuState.SelectingSingleAlly:
                            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)) && partySelectorIndex > 0)
                            {
                                currentPlayers[partySelectorIndex].SetSelected(false);

                                partySelectorIndex--;
                                SetSelectorToEntity(currentPlayers[partySelectorIndex]);
                            }

                            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W)) && partySelectorIndex < currentPlayers.Count - 1)
                            {
                                currentPlayers[partySelectorIndex].SetSelected(false);

                                partySelectorIndex++;
                                SetSelectorToEntity(currentPlayers[partySelectorIndex]);

                            }
                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                currentMenuState = previousMenuState;
                                entitySelector.gameObject.SetActive(false);
                                currentPlayers[partySelectorIndex].SetSelected(false);
                                ReturnPreviousMenuState();

                            }

                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                currentMenuState = MenuState.Idle;
                                entitySelector.gameObject.SetActive(false);
                                currentPlayers[partySelectorIndex].SetSelected(false);

                                battleEntities[entityIndex].SetTarget(currentPlayers[partySelectorIndex]);
                                if (previousMenuState == MenuState.ChoosingSpecialMove)
                                {
                                    SpecialData special = battleEntities[entityIndex].specialMoves[actionMenuController.GetSelection()];
                                    battleEntities[entityIndex].SetSpecialAttack(special);
                                    battleEntities[entityIndex].StartSpecialAttack();
                                }
                                else if (previousMenuState == MenuState.ChoosingItem)
                                {
                                    ItemData item = ((Player)battleEntities[entityIndex]).inventory.GetItem(actionMenuController.GetSelection());

                                    battleEntities[entityIndex].SetItem(item);
                                    battleEntities[entityIndex].StartSpecialAttack();
                                }
                                else
                                    battleEntities[entityIndex].StartMeleeAttack();
                                PAUSE = true;

                            }


                            break;
                        case MenuState.SelectingAllEnemies:
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                currentMenuState = MenuState.Idle;

                                List<BattleEntity> targets = new List<BattleEntity>();
                                foreach (Enemy enemy in currentEnemies)
                                {
                                    enemy.SetSelected(false);
                                    targets.Add(enemy);

                                }

                                battleEntities[entityIndex].SetMultipleTargets(targets);


                                if (previousMenuState == MenuState.ChoosingSpecialMove)
                                {
                                    SpecialData special = battleEntities[entityIndex].specialMoves[actionMenuController.GetSelection()];
                                    battleEntities[entityIndex].SetSpecialAttack(special);
                                    battleEntities[entityIndex].StartSpecialAttack();
                                }
                                else if (previousMenuState == MenuState.ChoosingItem)
                                {
                                    ItemData item = ((Player)battleEntities[entityIndex]).inventory.GetItem(actionMenuController.GetSelection());

                                    battleEntities[entityIndex].SetItem(item);
                                    battleEntities[entityIndex].StartSpecialAttack();
                                }
                                else
                                    battleEntities[entityIndex].StartMeleeAttack();
                                PAUSE = true;
                                
                            }
                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                currentMenuState = previousMenuState;
                                foreach(Enemy enemy in currentEnemies)
                                {
                                    enemy.SetSelected(false);

                                }
                                ReturnPreviousMenuState();

                            }
                            break;

                        case MenuState.SelectingAllAllies:
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                currentMenuState = MenuState.Idle;

                                List<BattleEntity> targets = new List<BattleEntity>();
                                foreach (Player player in currentPlayers)
                                {
                                    player.SetSelected(false);
                                    targets.Add(player);

                                }

                                battleEntities[entityIndex].SetMultipleTargets(targets);


                                if (previousMenuState == MenuState.ChoosingSpecialMove)
                                {
                                    SpecialData special = battleEntities[entityIndex].specialMoves[actionMenuController.GetSelection()];
                                    battleEntities[entityIndex].SetSpecialAttack(special);
                                    battleEntities[entityIndex].StartSpecialAttack();
                                }
                                else if (previousMenuState == MenuState.ChoosingItem)
                                {
                                    ItemData item = ((Player)battleEntities[entityIndex]).inventory.GetItem(actionMenuController.GetSelection());

                                    battleEntities[entityIndex].SetItem(item);
                                    battleEntities[entityIndex].StartSpecialAttack();
                                }
                                else
                                    battleEntities[entityIndex].StartMeleeAttack();
                                PAUSE = true;

                            }
                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                currentMenuState = previousMenuState;
                                foreach (Player player in currentPlayers)
                                {
                                    player.SetSelected(false);
                                }
                                ReturnPreviousMenuState();

                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
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
                    battleEntities[entityIndex].StartMeleeAttack();
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

    public bool CanUseSpecialMove(int index)
    {
        return battleEntities[entityIndex].specialMoves[index].SPcost <= battleEntities[entityIndex].SP;
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


    public void RemoveEnemy(Enemy enemy)
    {
        currentEnemies.Remove(enemy);
        if (currentEnemies.Count != 0 || currentBattleState == BattleState.Won) return;
        
        currentBattleState = BattleState.Won;
        //TODO Result Screen
        print("WON");
    }

    private void SetSelectorToEntity(BattleEntity entity)
    {
        var o = entity.gameObject;
        var position = o.transform.position;
        Vector3 dest = new Vector3(position.x, position.y + 5f, position.z);
        entitySelector.SetDestination(dest);

        entity.SetSelected(true);
    }


}
