using UnityEngine;
using System.Collections;

public class NextStage : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            SQLiteGameSystem playerUpdata = new SQLiteGameSystem();

            playerUpdata.PlayerPropertyUpdata();

            LoadStage.Globe.loadName = 3;
            Application.LoadLevel(1);
            Debug.Log("OK");
        }
    }
}
