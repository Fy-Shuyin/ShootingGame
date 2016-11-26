using UnityEngine;
using System.Collections;

public interface EnemyBossIState
{

    //进入状态是只呼出1次
    void Enter(EnemyBoss boss);

    //每针呼出状态
    void Execute(EnemyBoss boss);

    //离开状态时呼出1次
    void Exit(EnemyBoss boss);
}
