using UnityEngine;
using System.Collections;

public class EnemyBossMove : EnemyBossIState
{
    Vector3 moveTarget;
    float patrolTime;
    Vector3 bossVector, targetVector;

    public void Enter(EnemyBoss boss)
    {
        Debug.Log("Move");
        boss.bossStateNum = 1;
        patrolTime = Random.Range(boss.PatrolTimeMin, boss.PatrolTimeMax);
    }
    public void Execute(EnemyBoss boss)
    {
        boss.BossAnimetor.SetFloat("Speed", boss.BossAgent.speed / 10f);
        bossVector = boss.transform.position;

        var moveTarget = bossVector + boss.transform.forward;
        patrolTime -= Time.deltaTime;
        if (patrolTime <= 0)
            patrolTime = 0;
        NavMeshHit moveHit;
        if (boss.BossAgent.Raycast(moveTarget, out moveHit) || patrolTime == 0)
        {
            boss.ChangeState(new EnemyBossWait());
            return;
        }
        if (boss.targetGet == true)
        {
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

        boss.Move(moveTarget);
    }
    public void Exit(EnemyBoss boss)
    {
        boss.BossAnimetor.SetFloat("Speed", 0f);
        boss.BossAgent.Stop();
        boss.BossAgent.velocity = Vector3.zero;
    }
}
