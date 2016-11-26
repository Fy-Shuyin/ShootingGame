using UnityEngine;
using System.Collections;

public class EnemyBossTurn : EnemyBossIState
{
    Vector3 turnPoint;
    float turnAngle;
    Vector3 bossVector, targetVector;

    public void Enter(EnemyBoss boss)
    {
        Debug.Log("Turn");
        boss.bossStateNum = 3;
        turnAngle = Random.Range(45, 315);
        Quaternion turn = boss.transform.rotation * Quaternion.AngleAxis(turnAngle, Vector3.up);
        turnPoint = boss.transform.position + (turn * Vector3.forward);
        turnPoint.y = 0;
    }
    public void Execute(EnemyBoss boss)
    {
        boss.BossAnimetor.SetFloat("Turn",1);
        bossVector = boss.transform.position;

        var turnVoctor = turnPoint - bossVector;
        turnVoctor.y = 0f;

        var moveVector = Vector3.RotateTowards(boss.transform.forward, turnVoctor, Time.deltaTime * boss.RotateSpeed, 0f);
        moveVector.y = 0f;
        if (Vector3.Angle(moveVector, turnVoctor) <= 0.3f)
        {
            boss.ChangeState(new EnemyBossMove());
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
        boss.Turn(moveVector);
    }
    public void Exit(EnemyBoss boss)
    {
        boss.BossAnimetor.SetFloat("Turn", 0);
    }
}
