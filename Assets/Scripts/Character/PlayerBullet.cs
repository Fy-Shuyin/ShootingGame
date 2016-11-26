using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour
{
    public int damege = 10;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Wall" || collider.tag == "Enemy")
        {
            Destroy(gameObject);
            var prefab = Resources.Load("BulletSpark");
            GameObject bulletSpark = Instantiate(prefab) as GameObject;
            bulletSpark.transform.position = gameObject.transform.position;
            Destroy(bulletSpark,0.5f);
            //Debug.Log("bullet");
        }
    }
}
