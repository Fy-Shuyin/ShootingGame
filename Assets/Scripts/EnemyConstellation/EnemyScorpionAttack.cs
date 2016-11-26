using UnityEngine;
using System.Collections;

public class EnemyScorpionAttack : EnemyScorpionIState
{
    
    public void Enter(EnemyScorpion scorpion)
    {
        scorpion.scorpionAnimetor.speed = scorpion.AttackSpeed;
    }
    public void Execute(EnemyScorpion scorpion)
    {
        scorpion.transform.LookAt(scorpion.targetTransform);

        if (scorpion.attCoolDown == 0)
        {
            scorpion.scorpionAnimetor.Play("Attack");
            scorpion.Attack();
        }

        var moveplayer = scorpion.targetTransform.position;
        if (Vector3.Distance(moveplayer, scorpion.transform.position) >= scorpion.AttackRange)
        {
            scorpion.ChangeState(new EnemyScorpionChase());
            return;
        }

    }
    public void Exit(EnemyScorpion scorpion)
    {
        scorpion.scorpionAnimetor.speed = 1;
    }
}