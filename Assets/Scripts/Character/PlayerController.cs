using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    CharacterController playerController;
	NavMeshAgent playerAgent;
    AudioSource Sound;
    public AudioClip SEBullets;
    public AudioClip SEReload;

    Transform cameraTransform;
	Vector3 playerMove;
    float attackCoolDown;
    bool reloading;
    float gunType = 0.2f;
    float movespeed = 10f;
    public static Transform selfTransform;
    bool death;

	void Awake () 
	{
        playerController = GetComponent<CharacterController>();
		playerAgent = GetComponent<NavMeshAgent>();
        Sound = GetComponent<AudioSource>();
        selfTransform = this.transform;

        death = false;
    }

    void Update()
    {
        var attact = Input.GetKey(KeyCode.Mouse0);
        var change = Input.GetKeyDown(KeyCode.Q);
        var reload = Input.GetKeyDown(KeyCode.R);
        var slow = Input.GetKey(KeyCode.LeftShift);

        cameraTransform = gameObject.GetComponentInChildren<Camera>().transform;
        
        attackCoolDown -= Time.deltaTime;
        if (attackCoolDown <= 0)
        {
            attackCoolDown = 0;
        }
        if (attact)
        {
            if (attackCoolDown == 0)
            {
                Attact();
            }
        }

        if (reload && reloading==false)
        {
            if (SQLiteGameSystem.playerClips > 0 && SQLiteGameSystem.playerBullets != 30)
            {
                reloading = true;
                SQLiteGameSystem.playerClips--;
                attackCoolDown = 2.5f;
                StartCoroutine(Reload(2.0f));
                Sound.PlayOneShot(SEReload);
            }
        }

        if (slow)
        {
            movespeed = 3f;
        }
        else
            movespeed = 10f;

        Death();
        Move();
	}

	void Move()
	{
		float xm = 0,ym = 0f,zm = 0;
		if (Input.GetKey (KeyCode.W)) {
			zm += 1;
		}
		if (Input.GetKey (KeyCode.S)) {
			zm -= 1;
		}
		if (Input.GetKey (KeyCode.A)) {
			xm -= 1;
		}
		if (Input.GetKey (KeyCode.D)) {
			xm += 1;
		}
		playerMove = new Vector3(xm,ym,zm);
		if(playerMove.sqrMagnitude > 1f)
		{
			playerMove.Normalize();
		}
        
        playerAgent.Move(selfTransform.TransformDirection(playerMove) * movespeed * Time.deltaTime);
    }


    void Attact()
    {
        if (SQLiteGameSystem.playerBullets > 0)
        {
            SQLiteGameSystem.playerBullets--;
            var prefab = Resources.Load("Bullet");
            GameObject bullet = Instantiate(prefab) as GameObject;
            Destroy(bullet, 1.5f);
            var bulletRigid = bullet.GetComponent<Rigidbody>();
            bullet.transform.position = cameraTransform.position;
            Vector3 forse;
            forse = cameraTransform.forward;
            bullet.transform.LookAt(cameraTransform.position + cameraTransform.forward);
            bulletRigid.AddForce(forse * 300f);
            attackCoolDown = gunType;
            Sound.PlayOneShot(SEBullets);
        }
    }

    private IEnumerator Reload(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        SQLiteGameSystem.playerBullets = 30;
        reloading = false;
    }

    void Death()
    {
        if (SQLiteGameSystem.playerHp <= 0 && death == false)
        {
            death = true;
            LoadStage.Globe.loadName = 5;
            Application.LoadLevel(1);
            return;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "EnemyBullet")
        {
            var EnemyBullet = collider.GetComponent<EnemyBullet>();
            SQLiteGameSystem.playerHp -= EnemyBullet.damege;
            Destroy(collider.gameObject);
        }
        if (collider.tag == "EnemyAttack")
        {
            var Enemy = collider.transform.parent.parent.parent.parent.parent.parent.parent.parent;
            var EnemyAttack = Enemy.GetComponent<EnemyBoss>();
            SQLiteGameSystem.playerHp -= EnemyAttack.BossAtt;
        }
    }
}
