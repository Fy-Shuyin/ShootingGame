using UnityEngine;
using System.Collections;

public class EnemySuperRobot : MonoBehaviour {

    public NavMeshAgent superRobotAgent;
    public Animator superRobotAnimetor;

    public int superRobotId, superRobotHp, superRobotAct, superRobotDef;
    public float MoveSpeed;//移动速度
    public float SightRange;//追击距离
    public float AttackRange;//攻击距离
    public float AttackSpeed;//攻击速度
    public float StopRange;//停止范围
    public float Ken;//视野范围
    public float FieldOfVision;//视野角度
    public float VigilanceRange;//警戒范围
    public float RotateSpeed;//旋转速度
    public float WaitTimeMin;//最小等待时间
    public float WaitTimeMax;//最大等待时间
    public float PatrolTimeMin;//最小巡逻时间
    public float PatrolTimeMax;//最大巡逻时间
    public float HealthPointMin;//逃离生命值
    public float HealthPointMax;//远离生命值
    public Vector3 Direction;//正面方向向量
    public float Angle;//视角角度

    public Transform targetTransform;
    public bool targetGet, underAttack;
    public static bool attackOn;
    public enum superRobotState
    {
        Think , Move , Wait , Turn , Chase , Attack , Death 
    };
    public superRobotState State = superRobotState.Move;
    private float patrolTime;
    private float waitTime;
    private Vector3 turnPoint , stopPoint;
    private float turnAngle;
    private float attackCoolDown;
    void Start ()
    {
        superRobotAgent = GetComponent<NavMeshAgent>();
        superRobotAnimetor = GetComponent<Animator>();
        #region SuperRobotProperty
        SQLiteGameSystem superRobotProperty = new SQLiteGameSystem();
        superRobotProperty.EnemyProperty();
        superRobotHp = superRobotProperty.EnemyHp;
        superRobotAct = superRobotProperty.EnemyAct;
        superRobotDef = superRobotProperty.EnemyDef;
        MoveSpeed = superRobotProperty.MoveSpeed;
        SightRange = superRobotProperty.SightRange;
        AttackRange = superRobotProperty.AttackRange;
        AttackSpeed = superRobotProperty.AttackSpeed;
        StopRange = superRobotProperty.StopRange;
        Ken = superRobotProperty.Ken;
        FieldOfVision = superRobotProperty.FieldOfVision;
        VigilanceRange = superRobotProperty.VigilanceRange;
        RotateSpeed = superRobotProperty.RotateSpeed;
        WaitTimeMin = superRobotProperty.WaitTimeMin;
        WaitTimeMax = superRobotProperty.WaitTimeMax;
        PatrolTimeMin = superRobotProperty.PatrolTimeMin;
        PatrolTimeMax = superRobotProperty.PatrolTimeMax;
        HealthPointMin = superRobotProperty.HealthPointMin;
        HealthPointMax = superRobotProperty.HealthPointMax;
        superRobotAgent.speed = MoveSpeed;
        #endregion
    }

    void Update ()
    {
	    switch(State)
        {
            case superRobotState.Think:
                Think();
                break;
            case superRobotState.Move:
                Move(); //Debug.Log("Move");
                Think();
                break;
            case superRobotState.Wait:
                Wait(); //Debug.Log("Wait");
                Think();
                break;
            case superRobotState.Turn:
                Turn(); //Debug.Log("Turn");
                Think();
                break;
            case superRobotState.Chase:
                Chase();
                break;
            case superRobotState.Attack:
                Attack();
                break;
            case superRobotState.Death:
                Death();
                break;
        }
        attackCoolDown -= Time.deltaTime;
        if (attackCoolDown < 0)
        {
            attackCoolDown = 0;
        }
        if (superRobotHp <= 0)
        {
            State = superRobotState.Death;
        }   
	}

    void Think()
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, Ken, 1 << 8);
        if (cols.Length > 0)
        {
            foreach (Collider player in cols)
            {
                if (player.tag == "Player")
                {
                    targetGet = true;
                    targetTransform = player.transform;
                    //Debug.Log(targetGet);
                }
            }
        }
        else
            targetGet = false;
        if (targetGet)
        {
            Direction = targetTransform.position - transform.position;
            Angle = Vector3.Angle(Direction, transform.forward);
            if (Vector3.Distance(targetTransform.position, transform.position) < VigilanceRange)
            {
                State = superRobotState.Attack;
                return;
            }
            RaycastHit chaseHit;
            if (Physics.Raycast(transform.position, targetTransform.position - transform.position, out chaseHit, Ken) && Angle < FieldOfVision * 0.5f || underAttack == true || attackOn == true)
            {
                if (chaseHit.collider.tag == ("Player"))
                {
                    State = superRobotState.Chase;
                    //Debug.DrawLine(scorpion.transform.position, chaseHit.transform.position, Color.yellow, scorpion.Ken);
                    stopPoint = transform.position;
                    return;
                }
            }
        }
    }

    void Move()
    {
        superRobotAnimetor.Play("Walk");
        var moveTarget = transform.position + transform.forward;
        patrolTime -= Time.deltaTime;
        if (patrolTime <= 0)
            patrolTime = 0;
        NavMeshHit moveHit;
        if (superRobotAgent.Raycast(moveTarget, out moveHit)  || patrolTime == 0)
        {
            waitTime = Random.Range(WaitTimeMin, WaitTimeMax);
            State = superRobotState.Wait;
            return;
        }
        superRobotAgent.Resume();
        superRobotAgent.SetDestination(moveTarget);
    }

    void Wait()
    {
        superRobotAnimetor.Play("Wait");
        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
        {
            turnAngle = Random.Range(90, 270);
            Quaternion turn = transform.rotation * Quaternion.AngleAxis(turnAngle, Vector3.up);
            turnPoint = transform.position + (turn * Vector3.forward);
            turnPoint.y = 0;
            State = superRobotState.Turn;
            return;
        }
    }

    void Turn()
    {
        superRobotAnimetor.Play("Walk");

        var targetVoctor = turnPoint - transform.position;
        targetVoctor.y = 0f;

        var moveVector = Vector3.RotateTowards(transform.forward, targetVoctor, Time.deltaTime * RotateSpeed, 0f);
        moveVector.y = 0f;
        if (Vector3.Angle(moveVector, targetVoctor) <= 0.3f)
        {
            patrolTime = Random.Range(PatrolTimeMin, PatrolTimeMax);
            State = superRobotState.Move;
            return;
        }

        transform.forward = (moveVector);
    }

    void Chase()
    {
        superRobotAnimetor.Play("Walk");
        var moveplayer = targetTransform.position;
        if (Vector3.Distance(moveplayer, transform.position) < AttackRange)
        {
            State = superRobotState.Attack;
            return;
        }

        if (Vector3.Distance(moveplayer, transform.position) >= SightRange)
        {
            State = superRobotState.Wait;
            underAttack = false;
            return;
        }

        if (Vector3.Distance(stopPoint, transform.position) >= StopRange)
        {
            State = superRobotState.Wait;
            underAttack = false;
            return;
        }
        superRobotAgent.Resume();
        superRobotAgent.SetDestination(moveplayer);
    }

    void Attack()
    {
        superRobotAnimetor.Play("Walk");
        if (attackCoolDown == 0)
        {
            AttackOn();
            attackCoolDown = AttackSpeed;
        }
        var moveplayer = targetTransform.position;
        if (Vector3.Distance(moveplayer, transform.position) >= AttackRange)
        {
            State = superRobotState.Chase;
            return;
        }
    }

    void AttackOn()
    {
        transform.LookAt(targetTransform);
        var prefab = Resources.Load("EnemyBullet");
        var att = Instantiate(prefab) as GameObject;
        var bullet = att.GetComponent<EnemyBullet>();
        bullet.damege = superRobotAct;
        Destroy(att, 1.5f);
        var skillRigid = att.GetComponent<Rigidbody>();
        att.transform.position = new Vector3(transform.position.x, transform.position.y + 4f, transform.position.z);
        att.transform.LookAt(targetTransform);
        Vector3 forse;
        forse = att.transform.forward;
        skillRigid.AddForce(forse * 150);
    }

    void Death()
    {
        superRobotAgent.Stop();
        superRobotAnimetor.Play("Death");
        Destroy(gameObject,2f);
        gameObject.GetComponent<Collider>().enabled = false;
        return;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Bullet")
        {
            var bullet = collider.GetComponent<PlayerBullet>();
            superRobotHp -= (bullet.damege - superRobotDef);
            Destroy(collider.gameObject);
            if (underAttack == false)
            {
                underAttack = true;
            }
            //Debug.Log(underAttack);
        }
    }
}
