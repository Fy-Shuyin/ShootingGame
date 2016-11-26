using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;

public class SkillStorm : MonoBehaviour {

	private SQLiteHelper sql;
	SqliteDataReader reader;

	Animator SpellAnimator;

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

	void Start ()
	{
		sql = new SQLiteHelper("data source = GameSystem.db");
		reader = sql.ReadOneTable("SkillDatabase", new string[] { "SkillName" },new string[] {"=="},new string[] { "'Storm'" });
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

		SpellAnimator = GetComponent<Animator> ();
		SpellMethod = SkillType;
        SkillPose = SkillCastTime;
		Distance = SkillCastDistance;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (SkillChoose && SkillCoolDown == 0)
		{
			gameObject.transform.LookAt(ChoosePlayer.transform.position);
			SpellAnimator.SetTrigger (SkillAnimator);
			SkillCoolDown = SkillCastCD;
			StartCoroutine(MagicOn(SkillCastTime));
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
	}

	private IEnumerator MagicOn(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		SkillAtt = SkillTriggerPlayerAtt * SkillAttack;
		var prefab = Resources.Load(SkillEffect);
		var Magic = Instantiate(prefab) as GameObject;
		var skillRigid = Magic.GetComponent<Rigidbody>();
        Magic.AddComponent<SkillDamage>();
        Magic.transform.position = new Vector3(ChoosePlayer.transform.position.x , 0 ,ChoosePlayer.transform.position.z);
        Magic.transform.LookAt(ChoosePlayer.transform.position);
        Magic.GetComponent<SkillDamage>().GroupDamage = (int)SkillAtt;
        Magic.GetComponent<SkillDamage>().Trigger = TriggerPlayer;
        Magic.GetComponent<SkillDamage>().Target = ChoosePlayer;
        Destroy(Magic, SkillDuration);
    }
}
