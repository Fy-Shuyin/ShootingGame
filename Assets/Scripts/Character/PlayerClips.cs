using UnityEngine;
using System.Collections;

public class PlayerClips : MonoBehaviour
{
    float x,y,z;
    void Start()
    {
        x = Random.Range(0, 1f);
        y = Random.Range(0, 1f);
        z = Random.Range(0, 1f);
    }
    void Update()
    {
        transform.Rotate(new Vector3(x,y,z),5f);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            SQLiteGameSystem.playerClips++;
            Destroy(gameObject);
        }
    }
}
