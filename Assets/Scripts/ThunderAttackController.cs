using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAttackController : AttackAnimationController
{
    public GameObject thunderBolt;
    void Start()
    {
        foreach(BattleEntity entity in targets)
        {
            Vector3 targetPosition = entity.gameObject.transform.position;

            GameObject thunderBoltObj = Instantiate(thunderBolt);

            thunderBoltObj.transform.position = new Vector3(targetPosition.x, 8.65f, targetPosition.z);

            entity.ReceiveDamage(damage);
        }

        FinishAttackAnimation();
    }


}
