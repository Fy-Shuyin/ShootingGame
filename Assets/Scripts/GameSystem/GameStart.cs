using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            LoadStage.Globe.loadName = 2;
            Application.LoadLevel(1);
        }
	}
}
