using UnityEngine;
using System.Collections;

public class EnemyScorpionTurn : EnemyScorpionIState
{
    Vector3 turnPoint;
    float turnAngle;

    public void Enter(EnemyScorpion scorpion)
    {
        turnAngle = Random.Range(90,270);
        Quaternion turn = scorpion.transform.rotation * Quaternion.AngleAxis(turnAngle, Vector3.up);
        turnPoint = scorpion.transform.position + (turn * Vector3.forward) ;
        turnPoint.y = 0;
    }
    public void Execute(EnemyScorpion scorpion)
    {
        scorpion.scorpionAnimetor.Play("Turn");

        var targetVoctor = turnPoint - scorpion.transform.position;
        targetVoctor.y = 0f;

        var moveVector = Vector3.RotateTowards(scorpion.transform.forward, targetVoctor, Time.deltaTime * scorpion.RotateSpeed,0f);
        moveVector.y = 0f;
        if (Vector3.Angle(moveVector, targetVoctor) <= 0.3f)
        {
            scorpion.ChangeState(new EnemyScorpionMove());
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
                    //Debug.DrawLine(scorpion.transform.position, chaseHit.transform.position, Color.yellow, scorpion.Ken);
                    return;
                }
            }
        }

        scorpion.Turn(moveVector);
    }
    public void Exit(EnemyScorpion scorpion)
    { }
}