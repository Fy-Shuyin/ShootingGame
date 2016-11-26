using UnityEngine;
using System.Collections;

public class SkillBuf : MonoBehaviour
{
    public float Buf;
    public GameObject Trigger;
    public GameObject Target;
    public float BufTime;

    void Start()
    {
        var reprie = Trigger.GetComponent<EnemyBoss>();
        Buf *= reprie.BossAtt;
    }
    void Update()
    {
        BufTime -= Time.deltaTime;
        if (BufTime < 0)
            BufTime = 0;
        if (BufTime == 0)
        {
            var reprie = Trigger.GetComponent<EnemyBoss>();
            reprie.BossAtt = reprie._BossAtt;
            Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (Trigger.tag == Target.tag && collider.tag == Trigger.tag)
        {
            var reprie = Trigger.GetComponent<EnemyBoss>();
            if (gameObject.tag == "SkillAttUP")
            {
                reprie.BossAtt = (int)Buf;
            }
        }
    }
}
