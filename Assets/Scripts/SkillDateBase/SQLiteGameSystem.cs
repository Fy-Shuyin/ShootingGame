using UnityEngine;
using System.Collections;
using System.IO;
using Mono.Data.Sqlite;

public class SQLiteGameSystem
{

    private SQLiteHelper sql;
    SqliteDataReader reader;

    public static int playerHp, playerGuntype, playerClips, playerBullets;

    int constellationID;
    public string constellationName;
    public int EnemyId, EnemyHp, EnemyAct, EnemyDef;
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

    public ArrayList ReproducePX = new ArrayList();
    public ArrayList ReproducePY = new ArrayList();
    public ArrayList ReproducePZ = new ArrayList();
    public ArrayList LeaderReproducePX = new ArrayList();
    public ArrayList LeaderReproducePY = new ArrayList();
    public ArrayList LeaderReproducePZ = new ArrayList();
    void CreateTable()
    {
        /*sql = new SQLiteHelper("data source = GameSystem.db");
        sql.CreateTable("Point", new string[] { "Type","ReproducePX", "ReproducePY", "ReproducePZ"}, new string[] { "TEXT" , "FLOAD", "FLOAD", "FLOAD"});
        sql.InsertValues("Point", new string[] { "'Reproduce'", "'-8.2'", "'0.2'", "'20'" });
        sql.InsertValues("Point", new string[] { "'Patorl'", "'-8.2'", "'0.2'", "'20'" });

        sql.CreateTable("Property", new string[] { "HP", "GunType", "Clips", "Bullets" }, new string[] { "INTEGER", "INTEGER", "INTEGER", "INTEGER" });
        sql.InsertValues("Property", new string[] { "'100'", "'0'", "'2'", "'30'" });

        sql.CreateTable("EnemyProperty", new string[] { "ID", "NAME", "HP", "ACT", "DEF", "MoveSpeed", "SightRange", "AttackRange", "AttackSpeed", "StopRange", "Ken", "FieldOfVision", "VigilanceRange", "RotateSpeed", "WaitTimeMin", "WaitTimeMax", "HealthPointMin", "HealthPointMax" },
                                         new string[] { "INTEGER","TEXT", "INTEGER", "INTEGER", "INTEGER","FLOAD", "FLOAD", "FLOAD", "FLOAD", "FLOAD", "FLOAD", "FLOAD", "FLOAD", "FLOAD", "FLOAD", "FLOAD", "FLOAD", "FLOAD" });
        sql.InsertValues("EnemyProperty", new string[] { "'2'", "'Taurus'", "'25'", "'4'", "'3'", "'2'", "'8'", "'6'", "'2'", "'15'", "'8'", "'110'", "'5'", "'1'", "'3'", "'3'", "'5'", "'10'" });

        sql.CloseConnection();*/
    }
    public void PlayerPropertyInitialize()
    {
        sql = new SQLiteHelper("data source = GameSystem.db");
        reader = sql.ReadOneTable("Property",new string[] { "ID" },new string[] { "=="} , new string[] {"1"});
        while (reader.Read())
        {
            //HP
            playerHp = reader.GetInt32(reader.GetOrdinal("HP"));
            //GunType
            playerGuntype = reader.GetInt32(reader.GetOrdinal("GunType"));
            //Clips
            playerClips = reader.GetInt32(reader.GetOrdinal("Clips"));
            //Bullets
            playerBullets = reader.GetInt32(reader.GetOrdinal("Bullets"));
        }
        sql.CloseConnection();
    }

    public void PlayerPropertyUpdata()
    {
        sql = new SQLiteHelper("data source = GameSystem.db");
        sql.UpdateValues("Property", new string[] { "HP" }, new string[] { playerHp.ToString() }, "ID", "=", "'2'");
        sql.UpdateValues("Property", new string[] { "GunType" }, new string[] { playerGuntype.ToString() }, "ID", "=", "'2'");
        sql.UpdateValues("Property", new string[] { "Clips" }, new string[] { playerClips.ToString() }, "ID", "=", "'2'");
        sql.UpdateValues("Property", new string[] { "Bullets" }, new string[] { playerBullets.ToString() }, "ID", "=", "'2'");
        sql.CloseConnection();
    }

    public void PlayerPropertyRead()
    {
        sql = new SQLiteHelper("data source = GameSystem.db");
        reader = sql.ReadOneTable("Property", new string[] { "ID" }, new string[] { "==" }, new string[] { "2" });
        while (reader.Read())
        {
            //HP
            playerHp = reader.GetInt32(reader.GetOrdinal("HP"));
            //GunType
            playerGuntype = reader.GetInt32(reader.GetOrdinal("GunType"));
            //Clips
            playerClips = reader.GetInt32(reader.GetOrdinal("Clips"));
            //Bullets
            playerBullets = reader.GetInt32(reader.GetOrdinal("Bullets"));
        }
        sql.CloseConnection();
    }

    public void EnemyProperty()
    {
        sql = new SQLiteHelper("data source = GameSystem.db");
        constellationID = Random.Range(1, 6);
        reader = sql.ReadOneTable("EnemyProperty", new string[] { "ID" }, new string[] { "==" }, new string[] { constellationID.ToString() });
        while (reader.Read())
        {
            constellationName = reader.GetString(reader.GetOrdinal("NAME"));
            EnemyHp = reader.GetInt32(reader.GetOrdinal("HP"));
            EnemyAct = reader.GetInt32(reader.GetOrdinal("ACT"));
            EnemyDef = reader.GetInt32(reader.GetOrdinal("DEF"));
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
        }
        sql.CloseConnection();
    }

    public void ReproducePoint()
    {
        sql = new SQLiteHelper("data source = GameSystem.db");
        reader = sql.ReadOneTable("Point", new string[] { "Type" }, new string[] { "==" }, new string[] { "'Reproduce'" });
        while (reader.Read())
        {
            ReproducePX.Add(reader.GetFloat(reader.GetOrdinal("PointX")));
            ReproducePY.Add(reader.GetFloat(reader.GetOrdinal("PointY")));
            ReproducePZ.Add(reader.GetFloat(reader.GetOrdinal("PointZ")));
        }
        reader = sql.ReadOneTable("Point", new string[] { "Type" }, new string[] { "==" }, new string[] { "'Leader'" });
        while (reader.Read())
        {
            LeaderReproducePX.Add(reader.GetFloat(reader.GetOrdinal("PointX")));
            LeaderReproducePY.Add(reader.GetFloat(reader.GetOrdinal("PointY")));
            LeaderReproducePZ.Add(reader.GetFloat(reader.GetOrdinal("PointZ")));
        }
        sql.CloseConnection();
    }
}
