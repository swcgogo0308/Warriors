using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public enum State
    {
        Idle,
        Attacking,
        Tracking
    }

    public State state;

    public Transform playerTransform;

    public Transform rayTransform;

    public Animator anim;

    Vector3 direction;

    public int weaponFallenProbabile;

    public float moveSpeed;

    public float rotateSpeed;

    public float trackingRange;

    public float attackRange;


    public bool isRange;

    public int max_health;

    private int health;

    private int currentHealth;

    bool stateDelay;

    bool isAttack;

    float distance;

    public bool isDead;

    public bool isOnDamage;

    [Header("Weapon")]
    public Weapon myWeapon;
    // Use this for initialization

    private void Awake()
    {
        state = State.Tracking;
    }

    void Start () {

        currentHealth = max_health;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Weapon"))
                myWeapon = child.GetComponent<Weapon>();
        }

        switch(myWeapon.weaponGrade)
        {
            case Weapon.Grade.Normal :
                weaponFallenProbabile = 60;
                break;
            case Weapon.Grade.Epic :
                weaponFallenProbabile = 30;
                break;
            case Weapon.Grade.Speacial:
                weaponFallenProbabile = 10;
                break;
        }

        StartCoroutine(CheckState());
        StartCoroutine(CheckFoward());
    }
	
	// Update is called once per frame
	void Update () {
        if(myWeapon._isAttacking)
        {
            state = State.Tracking;
        }

        if (isDead) return;

        Move();
	}

    private void Move()
    {
        if (myWeapon._isAttacking) return;

        Vector3 movePos;

        direction = playerTransform.position - transform.position;

        direction.z = 0;

        direction.Normalize();

        switch (state)
        {
            case State.Idle:

                distance = Vector3.Distance(transform.position, playerTransform.position);

                Rotate(false);

                movePos = transform.position + (direction * 0);

                transform.position = movePos;

                if (distance <= attackRange) state = State.Attacking;

                break;

            case State.Tracking:

                Rotate(false);

                distance = Vector3.Distance(transform.position, playerTransform.position);

                if (distance < trackingRange - trackingRange * 0.1)
                {
                    movePos = transform.position + (-direction * moveSpeed * Time.deltaTime);

                    transform.position = movePos;
                }

                else if (distance <= trackingRange)
                {
                    state = State.Idle;
                }

                else if (distance > trackingRange)
                {
                    movePos = transform.position + (direction * moveSpeed * Time.deltaTime);

                    transform.position = movePos;
                }

                break;

            case State.Attacking:

                if (isRange) return;

                Rotate(false);

                movePos = transform.position + (direction * moveSpeed * Time.deltaTime);

                transform.position = movePos;

                break;
        }

    }

    void Rotate(bool isAttakck)
    {
        float angle = -1 * Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.y) + 90;

        if(!isAttack)
            transform.eulerAngles = new Vector3(0, 0, angle + 180.0f);
        else
            transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private IEnumerator CheckState()
    {
        while(true)
        {
            yield return null;

            if(state == State.Tracking)
            {

                if (stateDelay) continue;

                stateDelay = true;

                yield return new WaitForSeconds(Random.Range(2, 5));

                state = State.Attacking;

                stateDelay = false;
            }
        }
       
    }

    private IEnumerator CheckFoward()
    {
        while (true)
        {
            yield return null;

            Ray2D ray = new Ray2D(rayTransform.position, -transform.right);

            switch(state)
            {
                case State.Tracking:

                    RaycastHit2D trackingRayHit = Physics2D.Raycast(ray.origin, ray.direction, trackingRange);

                    if (trackingRayHit.collider == null || trackingRayHit.collider.tag != "Player")
                    {
                        isRange = false;
                        continue;
                    }

                    isRange = true;

                    break;

                case State.Attacking:

                    RaycastHit2D attackingRayHit = Physics2D.Raycast(ray.origin, ray.direction, attackRange);

                    if (attackingRayHit.collider == null || attackingRayHit.collider.tag != "Player")
                    {
                        isRange = false;
                        continue;
                    }

                    isRange = true;

                    Invoke("Attack", myWeapon.attackDelay);

                    yield return new WaitForSeconds(1f);

                    break;
            }

            
        }
    }

    private void Attack()
    {
        if (myWeapon._isAttacking || isDead) return;

        Rotate(true);

        myWeapon.Attack(isDead);

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void SetStrength(float strength)
    {
        currentHealth = (int)strength * 50 + currentHealth;
    }

    void Death()
    {
        isDead = true;
        StartCoroutine(DeathEffect());
    }

    IEnumerator DeathEffect()
    {
        anim.SetBool("isDead", true);


        yield return new WaitForSeconds(1.03f);

        if (Random.Range(1, 100) <= weaponFallenProbabile)
        {
			Transform fallenWeapon = new GameObject("FallenWeapon").transform;
			fallenWeapon.tag = "Fallen";
            fallenWeapon.position = myWeapon.transform.position;

            myWeapon.gameObject.transform.parent = fallenWeapon;
        }

        Destroy(gameObject);
    }

	public void WeaponBreak()
	{
		Destroy (myWeapon.gameObject);

		foreach (Transform child in transform)
		{
			if (child.CompareTag("Weapon"))
				myWeapon = child.GetComponent<Weapon>();
		}
	}
}
