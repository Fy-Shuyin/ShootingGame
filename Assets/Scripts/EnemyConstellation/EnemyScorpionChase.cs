using UnityEngine;
using System.Collections;

public class EnemyScorpionChase : EnemyScorpionIState
{
    Vector3 stopPoint;
    public void Enter(EnemyScorpion scorpion)
    {
        stopPoint = scorpion.transform.position;
        scorpion.MoveSpeed *= 1.5f;
        if (scorpion.leaderGet == true && scorpion.targetGet == true)
        {
            EnemySuperRobot.attackOn = true;
        }
    }
    public void Execute(EnemyScorpion scorpion)
    {
        scorpion.scorpionAnimetor.Play("Walk");
        var moveplayer = scorpion.targetTransform.position;
        RaycastHit chaseHit;
        if (Physics.Raycast(scorpion.transform.position, scorpion.targetTransform.position - scorpion.transform.position, out chaseHit, scorpion.Ken) && Vector3.Distance(moveplayer, scorpion.transform.position) < scorpion.AttackRange)
        {
            if (chaseHit.collider.tag == ("Player"))
            {
                scorpion.ChangeState(new EnemyScorpionAttack());
                return;
            }
        }

        if (Vector3.Distance(moveplayer, scorpion.transform.position) >= scorpion.SightRange)
        {
            scorpion.ChangeState(new EnemyScorpionMove());
            scorpion.MoveSpeed /= 1.5f;
            return;
        }

        if (Vector3.Distance(stopPoint, scorpion.transform.position) >= scorpion.StopRange)
        {
            scorpion.ChangeState(new EnemyScorpionMove());
            scorpion.MoveSpeed /= 1.5f;
            return;
        }

        scorpion.Move(moveplayer);
    }
    public void Exit(EnemyScorpion scorpion)
    {
        scorpion.targetGet = false;
        scorpion.underAttack = false;
        scorpion.scorpionAgent.Stop();
        scorpion.scorpionAgent.velocity = Vector3.zero;
    }
}