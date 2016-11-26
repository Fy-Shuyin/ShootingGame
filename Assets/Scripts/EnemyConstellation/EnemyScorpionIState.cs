using UnityEngine;
using System.Collections;

public interface EnemyScorpionIState
{
	//进入状态是只呼出1次
	void Enter(EnemyScorpion scorpion);

	//每针呼出状态
	void Execute (EnemyScorpion scorpion);

	//离开状态时呼出1次
	void Exit(EnemyScorpion scorpion);
}
