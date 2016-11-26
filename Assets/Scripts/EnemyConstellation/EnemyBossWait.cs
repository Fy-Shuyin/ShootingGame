using UnityEngine;
using System.Collections;

public class EnemyBossWait : EnemyBossIState
{
    float waittime;
    Vector3 bossVector, targetVector;

    public void Enter(EnemyBoss boss)
    {
        Debug.Log("Wait");
        boss.bossStateNum = 2;
        boss.targetGet = false;
        boss.underAttack = false;
        waittime = Random.Range(boss.WaitTimeMin, boss.WaitTimeMax);
    }
    public void Execute(EnemyBoss boss)
    {
        waittime -= Time.deltaTime;
        if (waittime <= 0)
        {
            boss.ChangeState(new EnemyBossTurn());
            return;
        }

        if (boss.targetGet == true)
        {
            bossVector = boss.transform.position;
            targetVector = boss.target.transform.position;

            boss.Direction = targetVector - bossVector;
            boss.Angle = Vector3.Angle(boss.Direction, boss.transform.forward);

            RaycastHit chaseHit;

            if (Physics.Raycast(bossVector, targetVector - bossVector, out chaseHit, boss.Ken) && Vector3.Distance(targetVector, bossVector) < boss.VigilanceRange)
            {
                if (chaseHit.collider.tag == ("Player"))
                {
                    boss.ChangeState(new EnemyBossChase());
                    return;
                }
            }

            if (Physics.Raycast(bossVector, targetVector - bossVector, out chaseHit, boss.Ken) && boss.Angle < boss.FieldOfVision * 0.5f || boss.underAttack == true)
            {
                if (chaseHit.collider.tag == ("Player"))
                {
                    boss.ChangeState(new EnemyBossChase());
                    return;
                }
            }
        }
    }
    public void Exit(EnemyBoss boss)
    { }
}
