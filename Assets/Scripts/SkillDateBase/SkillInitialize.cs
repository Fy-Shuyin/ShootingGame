using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;

public class SkillInitialize
{
	private SQLiteHelper sql;
	SqliteDataReader reader;

	public int[] SkillID;
	public string[]  SkillName;
	public string[] SkillType;//MonomerSkill,GroupSkill,SelfSkill
	public string[] SkillEffect;
	public float[] SkillAttack;
	public float[] SkillSpeed;
	public float[] SkillCastDistance;
	public float[] SkillCastRange;
	public float[] SkillCastTime;
	public float[] SkillDuration;
	public float[] SkillCastCD;
	public float[] SkillCoolDown;
	public int SkillQuantity;

	public void AllSkill()
	{
		SkillQuantity = 0;
		sql = new SQLiteHelper("data source = GameSystem.db");
		reader = sql.ReadFullTable("SkillDatabase");
		while (reader.Read())
		{
			SkillQuantity++;
		}

		SkillID = new int[SkillQuantity];
		SkillName = new string[SkillQuantity];
		SkillType = new string[SkillQuantity];
		SkillEffect = new string[SkillQuantity];
		SkillAttack = new float[SkillQuantity];
		SkillSpeed = new float[SkillQuantity];
		SkillCastDistance = new float[SkillQuantity];
		SkillCastRange = new float[SkillQuantity];
		SkillCastTime = new float[SkillQuantity];
		SkillDuration = new float[SkillQuantity];
		SkillCastCD = new float[SkillQuantity];
		SkillCoolDown = new float[SkillQuantity];
		SkillQuantity = 0;

		reader = sql.ReadFullTable("SkillDatabase");
		while (reader.Read())
		{
			SkillID[SkillQuantity] = reader.GetInt32(reader.GetOrdinal("ID"));
			SkillName[SkillQuantity] = reader.GetString(reader.GetOrdinal("SkillName"));
			SkillType[SkillQuantity] = reader.GetString(reader.GetOrdinal("SkillType"));
			SkillEffect[SkillQuantity] = reader.GetString(reader.GetOrdinal("SkillEffect"));
			SkillAttack[SkillQuantity] = reader.GetFloat(reader.GetOrdinal("SkillAttack"));
			SkillSpeed[SkillQuantity] = reader.GetFloat(reader.GetOrdinal("SkillSpeed"));
			SkillCastDistance[SkillQuantity] = reader.GetFloat(reader.GetOrdinal("SkillCastDistance"));
			SkillCastRange[SkillQuantity] = reader.GetFloat(reader.GetOrdinal("SkillCastRange"));
			SkillCastTime[SkillQuantity] = reader.GetFloat(reader.GetOrdinal("SkillCastTime"));
			SkillDuration[SkillQuantity] = reader.GetFloat(reader.GetOrdinal("SkillDuration"));
			SkillCastCD[SkillQuantity] = reader.GetFloat(reader.GetOrdinal("SkillCastCD"));
			SkillQuantity++;
		}
		sql.CloseConnection();
	}

	public static void Skills(string NAME,GameObject TRIGGER)
	{
		if (NAME == "TheLightOfSword")
		{
			TRIGGER.AddComponent<SkillTheLightOfSword> ();
		}
		if (NAME == "Storm") 
		{
			TRIGGER.AddComponent<SkillStorm> ();
		}
        if (NAME == "Healing")
        {
            TRIGGER.AddComponent<SkillHealing>();
        }
        if (NAME == "FlySword")
        {
            TRIGGER.AddComponent<SkillFlySword>();
        }
        if (NAME == "Fire")
        {
            TRIGGER.AddComponent<SkillFire>();
        }
        if (NAME == "AttUP")
        {
            TRIGGER.AddComponent<SkillAttUP>();
        }
    }

	public static void ChooseSkill(string NAME,ref string TYPE,GameObject TRIGGER,GameObject TARGET,string ANIMATOR,ref float DISTANCE ,ref int ATTACT,ref float COOLDOWN,bool TRUE,ref float POSE)
	{
		if (NAME == "TheLightOfSword")
		{
			SkillTheLightOfSword chooseskill = TRIGGER.GetComponent<SkillTheLightOfSword> ();
			TYPE = chooseskill.SpellMethod;
			chooseskill.TriggerPlayer = TRIGGER;
			chooseskill.ChoosePlayer = TARGET;
			chooseskill.SkillAnimator = ANIMATOR;
			DISTANCE = chooseskill.Distance;
			chooseskill.SkillTriggerPlayerAtt = ATTACT;
			chooseskill.SkillChoose = TRUE;
            POSE = chooseskill.SkillPose;
			COOLDOWN = chooseskill.SkillCoolDown;
		}
		if (NAME == "Storm") 
		{
			SkillStorm chooseskill = TRIGGER.GetComponent<SkillStorm> ();
			TYPE = chooseskill.SpellMethod;
			chooseskill.TriggerPlayer = TRIGGER;
			chooseskill.ChoosePlayer = TARGET;
			chooseskill.SkillAnimator = ANIMATOR;
			DISTANCE = chooseskill.Distance;
			chooseskill.SkillTriggerPlayerAtt = ATTACT;
			chooseskill.SkillChoose = TRUE ;
            POSE = chooseskill.SkillPose;
            COOLDOWN = chooseskill.SkillCoolDown;
		}
        if (NAME == "Healing")
        {
            SkillHealing chooseskill = TRIGGER.GetComponent<SkillHealing>();
            TYPE = chooseskill.SpellMethod;
            chooseskill.TriggerPlayer = TRIGGER;
            chooseskill.ChoosePlayer = TRIGGER;
            chooseskill.SkillAnimator = ANIMATOR;
            chooseskill.SkillChoose = TRUE;
            POSE = chooseskill.SkillPose;
            COOLDOWN = chooseskill.SkillCoolDown;
        }
        if (NAME == "FlySword")
        {
            SkillFlySword chooseskill = TRIGGER.GetComponent<SkillFlySword>();
            TYPE = chooseskill.SpellMethod;
            chooseskill.TriggerPlayer = TRIGGER;
            chooseskill.ChoosePlayer = TARGET;
            chooseskill.SkillAnimator = ANIMATOR;
            DISTANCE = chooseskill.Distance;
            chooseskill.SkillTriggerPlayerAtt = ATTACT;
            chooseskill.SkillChoose = TRUE;
            POSE = chooseskill.SkillPose;
            COOLDOWN = chooseskill.SkillCoolDown;
        }
        if (NAME == "Fire")
        {
            SkillFire chooseskill = TRIGGER.GetComponent<SkillFire>();
            TYPE = chooseskill.SpellMethod;
            chooseskill.TriggerPlayer = TRIGGER;
            chooseskill.ChoosePlayer = TARGET;
            chooseskill.SkillAnimator = ANIMATOR;
            DISTANCE = chooseskill.Distance;
            chooseskill.SkillTriggerPlayerAtt = ATTACT;
            chooseskill.SkillChoose = TRUE;
            POSE = chooseskill.SkillPose;
            COOLDOWN = chooseskill.SkillCoolDown;
        }
        if (NAME == "AttUP")
        {
            SkillAttUP chooseskill = TRIGGER.GetComponent<SkillAttUP>(); 
            TYPE = chooseskill.SpellMethod;
            chooseskill.TriggerPlayer = TRIGGER;
            chooseskill.ChoosePlayer = TRIGGER;
            chooseskill.SkillAnimator = ANIMATOR;
            chooseskill.SkillChoose = TRUE;
            POSE = chooseskill.SkillPose;
            COOLDOWN = chooseskill.SkillCoolDown;
        }
    }

}