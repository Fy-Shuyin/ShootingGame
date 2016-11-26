using UnityEngine;
using System.Collections;

public class EnemyBossAttack : EnemyBossIState
{
    Vector3 bossVector, targetVector;

    public void Enter(EnemyBoss boss)
    {
        Debug.Log("Attack");
        boss.bossStateNum = 5;
    }
    public void Execute(EnemyBoss boss)
    {
        bossVector = boss.transform.position;
        targetVector = boss.target.transform.position;

        if (boss.death == true)
        {
            boss.ChangeState(new EnemyBossDeath());
            return;
        }

        RaycastHit chaseHit;
        if (Physics.Raycast(bossVector, targetVector - bossVector, out chaseHit, boss.Ken) && Vector3.Distance(targetVector, bossVector) <= boss.AttackRange && boss.attackCoolDown == 0 && boss.PoseLoading == 0)
        {
            if (chaseHit.collider.tag == ("Player"))
            {
                if (boss.ModeFly == true)
                {
                    boss.BossAnimetor.SetTrigger("AttackFly");
                    boss.FlyAttact();
                }
                else
                    boss.BossAnimetor.SetTrigger("AttackFish");
                boss.attackCoolDown = boss.AttackSpeed;
            }
        }
        if (Vector3.Distance(targetVector, bossVector) > boss.AttackRange)
        {
            boss.ChangeState(new EnemyBossChase());
            return;
        }
            
    }
    public void Exit(EnemyBoss boss)
    { }
}
