using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGuns : MonoBehaviour {

    public Text hp , clips;
	void Start ()
    {
        hp.text = SQLiteGameSystem.playerHp.ToString();
        clips.text = SQLiteGameSystem.playerClips.ToString() + "ー " + SQLiteGameSystem.playerBullets.ToString();
    }
	
	void Update ()
    {
        hp.text = SQLiteGameSystem.playerHp.ToString();
        clips.text = SQLiteGameSystem.playerClips.ToString() + "ー " + SQLiteGameSystem.playerBullets.ToString();
    }
}
