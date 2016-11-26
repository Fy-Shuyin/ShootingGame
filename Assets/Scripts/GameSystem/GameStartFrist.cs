using UnityEngine;
using System.Collections;

public class GameStartFrist : MonoBehaviour {

    SQLiteGameSystem gameSystem = new SQLiteGameSystem();

    Collider[] cols;

    void Start ()
    {
        gameSystem.PlayerPropertyInitialize();
        gameSystem.ReproducePoint();
        Enemy();
    }
	
	void Update ()
    {
        cols = Physics.OverlapSphere(this.transform.position, 60, 1 << 10);
        if (cols.Length == 0)
        {
            GameObject door = GameObject.Find("Move (1)");
            Destroy(door);
        }

    }

    void Enemy ()
    {
        for (int i = 0; i < gameSystem.ReproducePX.Count; i++)
        {
            var enemyscorpion = Resources.Load("Scorpion");
            GameObject scorpion = Instantiate(enemyscorpion) as GameObject;
            scorpion.transform.position = new Vector3((float)gameSystem.ReproducePX[i], (float)gameSystem.ReproducePY[i], (float)gameSystem.ReproducePZ[i]);
            scorpion.GetComponent<NavMeshAgent>().enabled = true;
        }
        for (int j = 0; j < gameSystem.LeaderReproducePX.Count; j++)
        {
            //Debug.Log("superRobot");
            var enemySuperRobot = Resources.Load("SuperRobot");
            GameObject superRobot = Instantiate(enemySuperRobot) as GameObject;
            superRobot.transform.position = new Vector3((float)gameSystem.LeaderReproducePX[j], (float)gameSystem.LeaderReproducePY[j], (float)gameSystem.LeaderReproducePZ[j]);
            superRobot.GetComponent<NavMeshAgent>().enabled = true;
        }
    }


}
