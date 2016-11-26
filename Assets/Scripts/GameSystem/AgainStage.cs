using UnityEngine;
using System.Collections;

public class AgainStage : MonoBehaviour {
    
	void Start ()
    {
        StartCoroutine(WaitAndPrint(5.0F));
    }

    IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Application.LoadLevelAsync(0);
    }
}
