using UnityEngine;
using System.Collections;

public class GameStartSecend : MonoBehaviour {

    SQLiteGameSystem gameSystem = new SQLiteGameSystem();

    void Start ()
    {
        gameSystem.PlayerPropertyRead();
        InvokeRepeating("Clips", 30, 15);
    }

    void Clips()
    {
        var prefab = Resources.Load("Clips");
        var Clips = Instantiate(prefab) as GameObject;
        float x = Random.Range(-50f, 50f);
        float z = Random.Range(-50f, 50f);
        Clips.transform.position = new Vector3(x, 3f,z);
    }
}
