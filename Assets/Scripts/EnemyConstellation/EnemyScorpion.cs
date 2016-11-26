using UnityEngine;
using System.Collections;

public class EnemyScorpion : MonoBehaviour {

    EnemyScorpionIState scorpionState;
    public NavMeshAgent scorpionAgent;
    public Animator scorpionAnimetor;

    public int scorpionId, scorpionHp, scorpionAct, scorpionDef;
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
    public Transform leaderTransform;
    public bool targetGet, leaderGet , underAttack;
    public float attCoolDown;
    void Start ()
    {
        scorpionAgent = GetComponent<NavMeshAgent>();
        scorpionAnimetor = GetComponent<Animator>();
        #region ScorpionProperty
        SQLiteGameSystem ScorpionProperty = new SQLiteGameSystem();
        ScorpionProperty.EnemyProperty();
        scorpionHp = ScorpionProperty.EnemyHp;
        scorpionAct = ScorpionProperty.EnemyAct;
        scorpionDef = ScorpionProperty.EnemyDef;
        MoveSpeed = ScorpionProperty.MoveSpeed;
        SightRange = ScorpionProperty.SightRange;
        AttackRange = ScorpionProperty.AttackRange;
        AttackSpeed = ScorpionProperty.AttackSpeed;
        StopRange = ScorpionProperty.StopRange;
        Ken = ScorpionProperty.Ken;
        FieldOfVision = ScorpionProperty.FieldOfVision;
        VigilanceRange = ScorpionProperty.VigilanceRange;
        RotateSpeed = ScorpionProperty.RotateSpeed;
        WaitTimeMin = ScorpionProperty.WaitTimeMin;
        WaitTimeMax = ScorpionProperty.WaitTimeMax;
        PatrolTimeMin = ScorpionProperty.PatrolTimeMin;
        PatrolTimeMax = ScorpionProperty.PatrolTimeMax;
        HealthPointMin = ScorpionProperty.HealthPointMin;
        HealthPointMax = ScorpionProperty.HealthPointMax;
        scorpionAgent.speed = MoveSpeed;
        #endregion
        scorpionState = new EnemyScorpionTurn();
        scorpionState.Enter(this);
    }
	

	void Update ()
    {
        scorpionState.Execute(this);
        TargetPlayer();
        TargetLeader();
        attCoolDown -= Time.deltaTime;
        if (attCoolDown < 0)
            attCoolDown = 0;

        if (scorpionHp <= 0)
        {
            ChangeState(new EnemyScorpionDeath());
            gameObject.GetComponent<Collider>().enabled = false;
            Destroy(gameObject,2f);
        }
    }

    public void ChangeState(EnemyScorpionIState nextState)
    {
        //实行之前状态的最后1次
        scorpionState.Exit(this);

        scorpionState = nextState;

        //开始执行下一次状态
        scorpionState.Enter(this);
    }

    public void Move(Vector3 target)
    {
        scorpionAgent.Resume();
        scorpionAgent.SetDestination(target);
    }

    public void Turn(Vector3 vector)
    {
        transform.forward = vector;
    }

    public void Wait(Time time)
    {
    }

    public void Chase(Vector3 playerpoint)
    {
        scorpionAgent.Resume();
        scorpionAgent.SetDestination(playerpoint);
        
    }
    public void Attack()
    {
        attCoolDown = AttackSpeed;
        StartCoroutine(AttackOn(0.5f));
    }

    private IEnumerator AttackOn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        var prefab = Resources.Load("EnemyBullet");
        var att = Instantiate(prefab) as GameObject;
        var bullet = att.GetComponent<EnemyBullet>();
        bullet.damege = scorpionAct;
        Destroy(att,1.5f);
        var skillRigid = att.GetComponent<Rigidbody>();
        att.transform.position = new Vector3(transform.position.x, transform.position.y+3f, transform.position.z);
        att.transform.LookAt(targetTransform);
        Vector3 forse;
        forse = att.transform.forward;
        skillRigid.AddForce(forse * 150);
    }

    void TargetPlayer()
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, Ken, 1<<8);

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
        {
            targetGet = false;
            targetTransform = null;
        }
    }

    void TargetLeader()
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, Ken, 1 << 10);

        if (cols.Length > 0)
        {
            foreach (Collider leader in cols)
            {
                if (leader.tag == "Enemy")
                {
                    leaderGet = true;
                    leaderTransform = leader.transform;
                    //Debug.Log(leaderGet);
                }
            }
        }
        else
        {
            leaderGet = false;
            leaderTransform = null;
        }
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Bullet")
        {
            var bullet = collider.GetComponent<PlayerBullet>();
            scorpionHp -= (bullet.damege - scorpionDef);
            Destroy(collider.gameObject);
            if (underAttack == false)
            {
                underAttack = true;
            }
            //Debug.Log(underAttack);
        }
    }

    //便利的GUI表示
    void OnDrawGizmos()
    {
        Vector3 h = transform.position;
        Quaternion qr = transform.rotation;
        Quaternion right = transform.rotation * Quaternion.AngleAxis(FieldOfVision / 2, Vector3.up);
        Quaternion left = transform.rotation * Quaternion.AngleAxis(FieldOfVision / 2, Vector3.down);
        //追击距离线
        Gizmos.color = Color.white;
        Vector3 f = h + (qr * Vector3.forward) * SightRange;
        Gizmos.DrawLine(h, f);

        //视野范围线
        Gizmos.color = Color.blue;
        Vector3 l = h + (right * Vector3.forward) * Ken;
        Gizmos.DrawLine(h, l);
        Gizmos.color = Color.blue;
        Vector3 r = h + (left * Vector3.forward) * Ken;
        Gizmos.DrawLine(h, r);

        int smoothCount = 36;
        float drawangle = Mathf.PI * 2f / smoothCount;

        // 索敵範囲に線を表示
        Gizmos.color = Color.green;
        for (var i = 0; i < smoothCount; i++)
        {
            // 線の始点と終点を求める
            var startPosition = transform.position + new Vector3(Mathf.Sin(i * drawangle), 0f, Mathf.Cos(i * drawangle)) * VigilanceRange;
            var endPosition = transform.position + new Vector3(Mathf.Sin((i + 1) * drawangle), 0f, Mathf.Cos((i + 1) * drawangle)) * VigilanceRange;

            // 線の描画
            Gizmos.DrawLine(startPosition, endPosition);
        }


        //攻击範囲に線を表示
        Gizmos.color = Color.red;
        for (var i = 0; i < smoothCount; i++)
        {
            // 線の始点と終点を求める
            var startPosition = transform.position + new Vector3(Mathf.Sin(i * drawangle), 0f, Mathf.Cos(i * drawangle)) * AttackRange;
            var endPosition = transform.position + new Vector3(Mathf.Sin((i + 1) * drawangle), 0f, Mathf.Cos((i + 1) * drawangle)) * AttackRange;

            // 線の描画
            Gizmos.DrawLine(startPosition, endPosition);
        }
    }
}
