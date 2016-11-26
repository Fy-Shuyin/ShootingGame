using UnityEngine;
using System.Collections;

public class EnemyBossDeath : EnemyBossIState
{
    float time;
    public void Enter(EnemyBoss boss)
    {
        Debug.Log("Death");
        boss.BossAnimetor.SetLayerWeight(1, 0);
        boss.BossAnimetor.SetTrigger("Death");
        time = 10f;
    }
    public void Execute(EnemyBoss boss)
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            boss.ChangeState(new EnemyBossMove());
            return;
        }
    }
    public void Exit(EnemyBoss boss)
    {
        LoadStage.Globe.loadName = 4;
        Application.LoadLevel(1);
    }
}
