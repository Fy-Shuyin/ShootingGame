using UnityEngine;
using System.Collections;

public class EnemyBossChase : EnemyBossIState
{
    Vector3 stopPoint;
    Vector3 bossVector, targetVector;
    int i;

    public void Enter(EnemyBoss boss)
    {
        Debug.Log("Chase");
        boss.bossStateNum = 4;
        stopPoint = boss.transform.position;
        boss.BossAgent.speed *= 2f;
    }
    public void Execute(EnemyBoss boss)
    {
        boss.BossAnimetor.SetFloat("Speed", boss.BossAgent.speed / 10f);

        if (boss.targetGet == false)
        {
            boss.ChangeState(new EnemyBossWait());
            return;
        }
        if (boss.death == true)
        {
            boss.ChangeState(new EnemyBossDeath());
            return;
        }

        bossVector = boss.transform.position;
        targetVector = boss.target.transform.position;

        RaycastHit chaseHit;
        if (Physics.Raycast(bossVector, targetVector - bossVector, out chaseHit, boss.Ken) && Vector3.Distance(targetVector, bossVector) < boss.AttackRange)
        {
            if (chaseHit.collider.tag == ("Player"))
            {
                boss.ChangeState(new EnemyBossAttack());
                return;
            }
        }

        if (boss.PoseLoading == 0)
        {
            boss.Move(targetVector);
        }
        else
        {
            boss.ChangeState(new EnemyBossWait());
            return;
        }
    }

    public void Exit(EnemyBoss boss)
    {
        boss.BossAgent.speed /= 2f;
        boss.BossAnimetor.SetFloat("Speed", 0f);
        boss.BossAgent.Stop();
        boss.BossAgent.velocity = Vector3.zero;
    }
}
