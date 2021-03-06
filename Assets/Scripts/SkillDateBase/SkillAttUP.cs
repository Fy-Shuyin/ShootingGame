﻿using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;

public class SkillAttUP : MonoBehaviour {
    private SQLiteHelper sql;
    SqliteDataReader reader;

    Animator SpellAnimator;
    NavMeshAgent TriggerAgent;

    private int SkillID;
    private string SkillName;
    private string SkillType;//MonomerSkill,GroupSkill,SelfSkill
    private string SkillEffect;
    private float SkillAttack;
    private float SkillSpeed;
    private float SkillCastDistance;
    private float SkillCastRange;
    private float SkillCastTime;
    private float SkillDuration;
    private float SkillCastCD;

    public GameObject TriggerPlayer;
    public string SpellMethod;
    public GameObject ChoosePlayer;
    public string SkillAnimator;
    public float SkillPose;
    public bool SkillChoose;
    public float SkillCoolDown;
    public float SkillAtt;
    public float SkillTriggerPlayerAtt;
    public float Distance;

    GameObject Magic;
    Rigidbody skillRigid;
    Vector3 TargetPosition;
    Vector3 forse;
    bool EffectOn;

    void Start()
    {
        sql = new SQLiteHelper("data source = GameSystem.db");
        reader = sql.ReadOneTable("SkillDatabase", new string[] { "SkillName" }, new string[] { "==" }, new string[] { "'AttUP'" });
        while (reader.Read())
        {
            SkillID = reader.GetInt32(reader.GetOrdinal("ID"));
            SkillName = reader.GetString(reader.GetOrdinal("SkillName"));
            SkillType = reader.GetString(reader.GetOrdinal("SkillType"));
            SkillEffect = reader.GetString(reader.GetOrdinal("SkillEffect"));
            SkillAttack = reader.GetFloat(reader.GetOrdinal("SkillAttack"));
            SkillSpeed = reader.GetFloat(reader.GetOrdinal("SkillSpeed"));
            SkillCastDistance = reader.GetFloat(reader.GetOrdinal("SkillCastDistance"));
            SkillCastRange = reader.GetFloat(reader.GetOrdinal("SkillCastRange"));
            SkillCastTime = reader.GetFloat(reader.GetOrdinal("SkillCastTime"));
            SkillDuration = reader.GetFloat(reader.GetOrdinal("SkillDuration"));
            SkillCastCD = reader.GetFloat(reader.GetOrdinal("SkillCastCD"));
        }
        sql.CloseConnection();

        SpellAnimator = GetComponent<Animator>();
        TriggerAgent = GetComponent<NavMeshAgent>();
        SpellMethod = SkillType;
        SkillPose = SkillCastTime;
        Distance = SkillCastDistance;
    }


    void Update()
    {
        if (SkillChoose && SkillCoolDown == 0)
        {
            SpellAnimator.SetTrigger(SkillAnimator);
            SkillCoolDown = SkillCastCD;

            SkillAtt = SkillAttack;
            var prefab = Resources.Load(SkillEffect);
            Magic = Instantiate(prefab) as GameObject;
            EffectOn = true;
            Magic.AddComponent<SkillBuf>();
            Magic.GetComponent<SkillBuf>().Buf = SkillAtt;
            Magic.GetComponent<SkillBuf>().Trigger = TriggerPlayer;
            Magic.GetComponent<SkillBuf>().Target = ChoosePlayer;
            Magic.GetComponent<SkillBuf>().BufTime = SkillDuration;
            SkillChoose = false;
        }
        else
        {
            SkillCoolDown -= Time.deltaTime;
            if (SkillCoolDown < 0)
            {
                SkillCoolDown = 0;
            }
        }

        if (EffectOn == true)
        {
            if (SkillCoolDown <= 15)
            { EffectOn = false; }
            Magic.transform.position = new Vector3(TriggerPlayer.transform.position.x, TriggerPlayer.transform.position.y, TriggerPlayer.transform.position.z);
        }
    }
}
