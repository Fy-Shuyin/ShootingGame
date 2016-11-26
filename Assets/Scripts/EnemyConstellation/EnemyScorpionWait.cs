using UnityEngine;
using System.Collections;

public class EnemyScorpionWait : EnemyScorpionIState
{
    float waittime;
    public void Enter(EnemyScorpion scorpion)
    {
        waittime = Random.Range(scorpion.WaitTimeMin, scorpion.WaitTimeMax);
    }
    public void Execute(EnemyScorpion scorpion)
    {
        scorpion.scorpionAnimetor.Play("Idle");
        waittime -= Time.deltaTime;
        if (waittime <= 0)
        {
            scorpion.ChangeState(new EnemyScorpionTurn());
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
                if (chaseHit.collider.tag == ("Player") )
                {
                    scorpion.ChangeState(new EnemyScorpionChase());
                    //Debug.DrawLine(scorpion.transform.position, chaseHit.transform.position, Color.yellow, scorpion.Ken);
                    return;
                }
            }
            
        }
    }
    public void Exit(EnemyScorpion scorpion)
    { }
}