using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationController : MonoBehaviour
{

    protected List<BattleEntity> targets;
    protected int damage;
    private StatusEffect status;

    public void SetTarget(List<BattleEntity> targets)
    {
        this.targets = targets;
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    public void FinishAttackAnimation()
    { 

        if(status.status != null)
        {
            foreach (BattleEntity entity in targets)
            {
                if (Random.Range(0, 100) <= status.chanceOfSuccess)
                    entity.AddStatus(status.status);
                else if(damage <= 0)
                {
                    Instantiate(BattleController.instance.missPrefab, new Vector3(entity.transform.position.x, entity.transform.position.y + 1.5f, entity.transform.position.z), Quaternion.identity, GameObject.Find("WorldSpaceCanvas").transform);

                }
            }
        }
        Destroy(gameObject);

    }
    public void SetStatus(StatusEffect status)
    {
        this.status = status;
    }
}
