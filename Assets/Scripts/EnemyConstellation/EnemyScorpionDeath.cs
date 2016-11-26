using UnityEngine;
using System.Collections;

public class EnemyScorpionDeath : EnemyScorpionIState
{

    public void Enter(EnemyScorpion scorpion)
    {
        scorpion.scorpionAgent.Stop();
        scorpion.scorpionAnimetor.speed = 1f;
        scorpion.scorpionAnimetor.Play("Death");
        return;
    }
    public void Execute(EnemyScorpion scorpion)
    { }
    public void Exit(EnemyScorpion scorpion)
    { }
}