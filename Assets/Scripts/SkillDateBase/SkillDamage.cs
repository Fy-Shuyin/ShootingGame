using UnityEngine;
using System.Collections;

public class SkillDamage : MonoBehaviour {
  
    public int MonomeDamage;
    public int GroupDamage;
    public GameObject Trigger;
    public GameObject Target;
    float DamageTime;
    
    void Update()
    {
        DamageTime -= Time.deltaTime;
        if (DamageTime < 0)
            DamageTime = 0;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (Trigger.tag == Target.tag && collider.tag == Trigger.tag)
        {
            var reprie = Trigger.GetComponent<EnemyBoss>();
            
            if (gameObject.tag == "SkillHealing")
            {
                reprie.BossHp += MonomeDamage;
            }

        }
        if (Trigger.tag != Target.tag && collider.tag == Target.tag)
        {
            SQLiteGameSystem.playerHp -= MonomeDamage;
        }
    }
    void OnTriggerStay(Collider collider)
    {
        if (Trigger.tag == Target.tag && collider.tag == Trigger.tag)
        {
            if (DamageTime == 0)
            {
                DamageTime = 1f;
            }
        }
        if (Trigger.tag != Target.tag && collider.tag == Target.tag)
        {
            if (DamageTime == 0)
            {
                SQLiteGameSystem.playerHp -= GroupDamage;
                DamageTime = 1f;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
    }

}
