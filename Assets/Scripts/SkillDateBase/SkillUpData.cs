using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;

public class SkillUpData : MonoBehaviour
{
    SQLiteHelper sql;
    SqliteDataReader reader;

	ArrayList DataBaseSkillName = new ArrayList();
	ArrayList DataBaseSkillType = new ArrayList();
	ArrayList BossSkillName = new ArrayList();
	ArrayList BossSkillType = new ArrayList();

    int DataBaseSkillNum, BossSkillNum;

    public void Start()
    {
        DataBaseSkillNum = 0;
        BossSkillNum = 0;

        sql = new SQLiteHelper("data source = GameSystem.db");
        reader = sql.ReadFullTable("SkillDatabase");
        while (reader.Read())
        {
            DataBaseSkillName.Add(reader.GetString(reader.GetOrdinal("SkillName")));
            DataBaseSkillType.Add(reader.GetString(reader.GetOrdinal("SkillType")));
            DataBaseSkillNum++;
        }

        reader = sql.ReadFullTable("EnemyBossSkill");
        while (reader.Read())
        {
            BossSkillName.Add(reader.GetString(reader.GetOrdinal("SkillName")));
            BossSkillType.Add(reader.GetString(reader.GetOrdinal("SkillType")));
			Debug.Log (reader.GetString(reader.GetOrdinal("SkillName")));
            BossSkillNum++;
        }

        if (BossSkillNum < DataBaseSkillNum)
        {
            foreach (string NAME in BossSkillName)
            {
                foreach (string _NAME in DataBaseSkillName)
                {
                    if (NAME == _NAME)
                    {
                        DataBaseSkillName.Remove(_NAME);
                        break;
                    }
                }
            }
            foreach (string TYPE in BossSkillType)
            {
                foreach (string _TYPE in DataBaseSkillType)
                {
                    if (TYPE == _TYPE)
                    {
                        DataBaseSkillName.Remove(_TYPE);
                        break;
                    }
                }
            }
			int i = Random.Range(0, DataBaseSkillName.Count);
			sql.InsertValues("EnemyBossSkill", new string[] {DataBaseSkillName[i].ToString(),DataBaseSkillType[i].ToString()});
        }
		//sql.DeleteValuesAND("EnemyBossSkill", new string[]{"SkillName"}, new string[]{"="}, new string[]{"''"});
        sql.CloseConnection();
    }
}
