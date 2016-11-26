using UnityEngine; 
using System.Collections;

public class EnemyScorpionMove : EnemyScorpionIState {
    Vector3 moveTarget;
    float patrolTime;
    public void Enter(EnemyScorpion scorpion)
    {
        patrolTime = Random.Range(scorpion.PatrolTimeMin,scorpion.PatrolTimeMax);
        //moveTarget = scorpion.transform.position + (scorpion.transform.forward * scorpion.Ken);
    }
    public void Execute(EnemyScorpion scorpion)
    {
        scorpion.scorpionAnimetor.Play("Walk");

        var moveTarget = scorpion.transform.position + scorpion.transform.forward;
        patrolTime -= Time.deltaTime;
        if (patrolTime <= 0)
            patrolTime = 0;
        NavMeshHit moveHit;
        if (scorpion.scorpionAgent.Raycast(moveTarget, out moveHit) || patrolTime == 0)
        {
            scorpion.ChangeState(new EnemyScorpionWait());
            return;
        }
        if (scorpion.targetGet == true)
        {
            scorpion.Direction = scorpion.targetTransform.position - scorpion.transform.position;
            scorpion.Angle = Vector3.Angle(scorpion.Direction, scorpion.transform.forward);
            if (Vector3.Distance(scorpion.targetTransform.position, scorpion.transform.position) < scorpion.VigilanceRange)
            {
                scorpion.ChangeState(new EnemyScorpionAttack());
                return;
            }
            RaycastHit chaseHit;
            if (Physics.Raycast(scorpion.transform.position, scorpion.targetTransform.position - scorpion.transform.position, out chaseHit, scorpion.Ken) && scorpion.Angle < scorpion.FieldOfVision * 0.5f || scorpion.underAttack == true || EnemySuperRobot.attackOn == true)
            {
                if (chaseHit.collider.tag == ("Player"))
                {
                    scorpion.ChangeState(new EnemyScorpionChase());
                    Debug.DrawLine(scorpion.transform.position, chaseHit.transform.position, Color.yellow, scorpion.Ken);
                    return;
                }
            }
        }

        scorpion.Move(moveTarget);
    }
    public void Exit(EnemyScorpion scorpion)
    {
        scorpion.scorpionAgent.Stop();
        scorpion.scorpionAgent.velocity = Vector3.zero;
    }
}
