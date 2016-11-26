using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;

public class EnemyBossSkill: MonoBehaviour
{
    private SQLiteHelper sql;
    SqliteDataReader reader;

	public string[] SkillName;
	public string[] SkillType;
	public int SkillQuantity;

    public void Start()
    {
        SkillQuantity = 0;
        sql = new SQLiteHelper("data source = GameSystem.db");
        reader = sql.ReadFullTable("EnemyBossSkill");
        while (reader.Read())
        {
        	SkillQuantity++;
        }
		SkillName = new string[SkillQuantity];
		SkillType = new string[SkillQuantity];
		SkillQuantity = 0;

        reader = sql.ReadFullTable("EnemyBossSkill");
        while (reader.Read())
		{
			SkillName[SkillQuantity] = reader.GetString(reader.GetOrdinal("SkillName"));
			SkillType[SkillQuantity] = reader.GetString (reader.GetOrdinal ("SkillType"));
			SkillInitialize.Skills (SkillName[SkillQuantity],gameObject);
			SkillQuantity++;
        }
		sql.CloseConnection ();
    }
		
}
