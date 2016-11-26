using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;

public class EnemyBoss : MonoBehaviour {

    private SQLiteHelper sql;
    SqliteDataReader reader;
    
    EnemyBossIState bossState;
	EnemyBossSkill bossSkill;

    public int bossStateNum;//Move-1,Wait-2,Turn-3,Chase-4,Att-5

    public NavMeshAgent BossAgent;
    public Animator BossAnimetor;

    public int BossId, BossHp, BossAtt, BossDef;
    public int _BossHp,_BossAtt;
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
    public float PoseLoading;
    float _PoseLoading;

    public bool death;

    public bool ModeFly;
    bool ModeFlyChance;
    GameObject FlySystem;

    public GameObject target;
    public bool targetGet, underAttack;
    public float attackCoolDown;
	public string SkillChooseName;
	public string SkillChooseType;
	public string SkillAnimator;
	public float SkillDistance;
    float _SkillDistance;
    public float SkillCoolDown;
    public ArrayList bossToTarget = new ArrayList();
    

    void Start () {
        BossAgent = GetComponent<NavMeshAgent>();
        BossAnimetor = GetComponent<Animator>();
		bossSkill = gameObject.GetComponent<EnemyBossSkill>();

        FlySystem = GameObject.Find("FlySystem");
        FlySystem.SetActive(false);

        BossProperty();

        bossState = new EnemyBossWait();
        bossState.Enter(this);
    }
	
	void Update ()
    {
        bossState.Execute(this);
        attackCoolDown -= Time.deltaTime;
        if (attackCoolDown <= 0)
            attackCoolDown = 0;
        PoseLoading -= Time.deltaTime;
        if (PoseLoading <= 0)
            PoseLoading = 0;
        TargetPlayer();
		SkillSpell ();
        Death();
        
    }

    public void ChangeState(EnemyBossIState nextState)
    {
        bossState.Exit(this);

        bossState = nextState;
        
        bossState.Enter(this);
    }

    public void Move(Vector3 target)
    {
        BossAgent.Resume();
        BossAgent.SetDestination(target);
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
        BossAgent.Resume();
        BossAgent.SetDestination(playerpoint);
    }

    public void TargetPlayer()
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, Ken, 1 << 8);
        if (cols.Length > 0)
        {
            foreach (Collider player in cols)
            {
                if (player.tag == "Player")
                {
                    bossToTarget.Add(Vector3.Distance(transform.position, player.transform.position));
                    for (int i = 1;i<bossToTarget.Count;i++)
                    {
                        if ((float)bossToTarget[i] < (float)bossToTarget[i - 1])
                        {
                            targetGet = true;
                            target = player.gameObject;
                            //Debug.Log(targetGet);
                        }
                    }
                }
            }
        }
        else
        {
            targetGet = false;
            target = null;
            underAttack = false;
        }
    }

    public void BossProperty()
    {
        sql = new SQLiteHelper("data source = GameSystem.db");
        reader = sql.ReadOneTable("BossProperty", new string[] { "ID" }, new string[] { "==" }, new string[] {"'1'"});
        while (reader.Read())
        {
            BossHp = reader.GetInt32(reader.GetOrdinal("HP"));
            BossAtt = reader.GetInt32(reader.GetOrdinal("ACT"));
            BossDef = reader.GetInt32(reader.GetOrdinal("DEF"));
            MoveSpeed = reader.GetFloat(reader.GetOrdinal("MoveSpeed"));
            SightRange = reader.GetFloat(reader.GetOrdinal("SightRange"));
            AttackRange = reader.GetFloat(reader.GetOrdinal("AttackRange"));
            AttackSpeed = reader.GetFloat(reader.GetOrdinal("AttackSpeed"));
            StopRange = reader.GetFloat(reader.GetOrdinal("StopRange"));
            Ken = reader.GetFloat(reader.GetOrdinal("Ken"));
            FieldOfVision = reader.GetFloat(reader.GetOrdinal("FieldOfVision"));
            VigilanceRange = reader.GetFloat(reader.GetOrdinal("VigilanceRange"));
            RotateSpeed = reader.GetFloat(reader.GetOrdinal("RotateSpeed"));
            WaitTimeMin = reader.GetFloat(reader.GetOrdinal("WaitTimeMin"));
            WaitTimeMax = reader.GetFloat(reader.GetOrdinal("WaitTimeMax"));
            PatrolTimeMin = reader.GetFloat(reader.GetOrdinal("PatrolTimeMin"));
            PatrolTimeMax = reader.GetFloat(reader.GetOrdinal("PatrolTimeMax"));
            HealthPointMin = reader.GetFloat(reader.GetOrdinal("HealthPointMin"));
            HealthPointMax = reader.GetFloat(reader.GetOrdinal("HealthPointMax"));
            BossAgent.speed = MoveSpeed;
        }
        sql.CloseConnection();
        _BossHp = BossHp;
        _BossAtt = BossAtt;
    }

    void Death()
    {
        if (BossHp <= 0 && death == false)
        {
            death = true;
        }
    }

    public IEnumerator DeathEnd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        LoadStage.Globe.loadName = 4;
        Application.LoadLevel(1);
    }

    void SkillSpell()
    {
        if (bossStateNum == 4 || bossStateNum == 5)
        {
            SpellON();
            SpellSelf();
        }
    }

    void SpellON()
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, Ken, 1 << 8);
        if (cols.Length == 1)
        {
            for (int i = 0; i < bossSkill.SkillQuantity; i++)
            {
                SkillInitialize.ChooseSkill(bossSkill.SkillName[i], ref bossSkill.SkillType[i], gameObject, target, SkillAnimator, ref _SkillDistance, ref BossAtt, ref SkillCoolDown, false, ref _PoseLoading);
                if (bossSkill.SkillType[i] == "MonomerSkill" && SkillCoolDown == 0 && Vector3.Distance(gameObject.transform.position, target.transform.position) <= _SkillDistance && PoseLoading == 0)
                {
                    SkillChooseName = bossSkill.SkillName[i];
                    SkillChooseType = bossSkill.SkillType[i];
                    IsSkill();
                }
                else
                if (bossSkill.SkillType[i] == "GroupSkill" && SkillCoolDown == 0 && Vector3.Distance(gameObject.transform.position, target.transform.position) <= _SkillDistance && PoseLoading == 0)
                {
                    SkillChooseName = bossSkill.SkillName[i];
                    SkillChooseType = bossSkill.SkillType[i];
                    IsSkill();
                }

            }
        }
        if (cols.Length > 1)
        {
            for (int i = 0; i < bossSkill.SkillQuantity; i++)
            {
                SkillInitialize.ChooseSkill(bossSkill.SkillName[i], ref bossSkill.SkillType[i], gameObject, target, SkillAnimator, ref _SkillDistance, ref BossAtt, ref SkillCoolDown, false, ref _PoseLoading);
                if (bossSkill.SkillType[i] == "GroupSkill" && SkillCoolDown == 0 && Vector3.Distance(gameObject.transform.position, target.transform.position) <= _SkillDistance && PoseLoading == 0)
                {
                    SkillChooseName = bossSkill.SkillName[i];
                    SkillChooseType = bossSkill.SkillType[i];
                    IsSkill();
                }
                else
                if (bossSkill.SkillType[i] == "MonomerSkill" && SkillCoolDown == 0 && Vector3.Distance(gameObject.transform.position, target.transform.position) <= _SkillDistance && PoseLoading == 0)
                {
                    SkillChooseName = bossSkill.SkillName[i];
                    SkillChooseType = bossSkill.SkillType[i];
                    IsSkill();
                }

            }
        }
    }

    void SpellSelf()
    {
        if (underAttack == true)
        {
            if (BossHp < _BossHp / 2 && ModeFly == false && ModeFlyChance== false)
            {
                if (Random.Range(0, 101) < 20)
                {
                
                    BossAnimetor.SetLayerWeight(1, 1);
                    ModeFly = true;
                    AttackRange *= 4f;
                    FlySystem.SetActive(true);
                    ModeFlyChance = true;
                }
                else
                {
                    ModeFlyChance = true;
                }
            }
            
            
            if (BossHp < _BossHp * 0.7f)
            {
                for (int i = 0; i < bossSkill.SkillQuantity; i++)
                {
                    SkillInitialize.ChooseSkill(bossSkill.SkillName[i], ref bossSkill.SkillType[i], gameObject, target, SkillAnimator, ref _SkillDistance, ref BossAtt, ref SkillCoolDown, false, ref _PoseLoading);
                    if (bossSkill.SkillType[i] == "SelfSkill" && SkillCoolDown == 0 && PoseLoading == 0)
                    {
                        SkillChooseName = bossSkill.SkillName[i];
                        SkillChooseType = bossSkill.SkillType[i];
                        IsSkill();
                    }
                }
            }
        }
    }

    void IsSkill()
    {
        if (ModeFly == true)
        {
            SkillAnimator = "AttackFly";
        }
        else
        {
            if (SkillChooseType == "MonomerSkill")
            {
                SkillAnimator = "AttackPush";
            }
            if (SkillChooseType == "GroupSkill")
            {
                SkillAnimator = "AttackHeavyPunch";
            }
            if (SkillChooseType == "SelfSkill")
            {
                SkillAnimator = "AttackFish";
            }
        }

        SkillInitialize.ChooseSkill(SkillChooseName, ref SkillChooseType, gameObject, target, SkillAnimator, ref _SkillDistance, ref BossAtt, ref SkillCoolDown, true, ref _PoseLoading);
        PoseLoading = _PoseLoading;
        SkillDistance = _SkillDistance;
    }

    public void FlyAttact()
    {
        var prefab = Resources.Load("BossAttack");
        var flyatt = Instantiate(prefab) as GameObject;
        var flyattRigid = flyatt.GetComponent<Rigidbody>();
        Destroy(flyatt, 3f);
        flyatt.AddComponent<EnemyBullet>();
        flyatt.GetComponent<EnemyBullet>().damege = BossAtt;
        flyatt.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 6f, gameObject.transform.position.z);
        flyatt.transform.LookAt(target.transform.position);
        var forse = flyatt.transform.forward;
        flyattRigid.AddForce(forse * 150);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Bullet")
        {
            var bullet = collider.GetComponent<PlayerBullet>();
            BossHp -= (bullet.damege - BossDef);
            Destroy(collider.gameObject);
            if (underAttack == false)
            {
                underAttack = true;
            }
            //Debug.Log(underAttack);
        }
    }

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
