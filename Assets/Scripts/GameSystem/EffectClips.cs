using UnityEngine;
using System.Collections;

public class EffectClips : MonoBehaviour {

    public GameObject Origin;
    float x, z, dis,angle;

    void Start()
    {
        dis = 1.5f;
        angle = 0;
    }

	void Update ()
    {
        if (angle >= 360)
        {
            angle = 0;
        }
        else
        {
            angle += 200*Time.deltaTime;
        }
        x = dis * Mathf.Sin(angle * Mathf.PI/180);
        z = dis * Mathf.Cos(angle * Mathf.PI / 180);
        transform.position = new Vector3(Origin.transform.position.x - x, Origin.transform.position.y + 1, Origin.transform.position.z - z);
    }
}
