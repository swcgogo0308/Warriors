using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Enemy : MonoBehaviour {

    public enum State
    {
        Idle,
        Attacking,
        Tracking
    }

    public State state;

    private SpriteRenderer mySprite;

    public Transform playerTransform;

    public Transform rayTransform;

    public Animator anim;

    private Color myColor;

    Vector3 direction;

    public int weaponFallenProbabile;

    public float moveSpeed;

    public float rotateSpeed;

    public float trackingRange;

    public float attackRange;

    public float flashSpeed;

    public bool isRange;

    public int max_health;

    private int health;

    private int currentHealth;

    bool stateDelay;

    bool isAttack;

	bool isBackMoving;

    bool isTrackingMove;

    bool isDamage;

    float distance;

    public bool isDead;

    public bool isOnDamage;

    [Header("Weapon")]
    public Weapon myWeapon;

    // Use this for initialization

    private void Awake()
    {
        state = State.Tracking;
        myColor = GetComponent<SpriteRenderer>().color;
    }

    void Start () {

        mySprite = GetComponent<SpriteRenderer>();

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

        DamageEffect();


        if (myWeapon._isAttacking)
        {
            state = State.Tracking;
			StartCoroutine(BackMovingDelay());
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
            
		    case State.Tracking:

			    Rotate (false);

			    distance = Vector3.Distance (transform.position, playerTransform.position);

                if (isBackMoving)
                {
                    if (distance > trackingRange) return;

                    movePos = transform.position + (-direction * moveSpeed * Time.deltaTime);

                    transform.position = movePos;

                }

                else
                {
                    if (distance > trackingRange)
                    {

                        if (isTrackingMove) return;

                        movePos = transform.position + (direction * moveSpeed * Time.deltaTime);

                        transform.position = movePos;
                    }

                    if(distance <= attackRange * 2f)
                    {
                        state = State.Attacking;
                    }

                    if (distance <= trackingRange)
                    {
                        StartCoroutine(TrackingDelay());
                    }
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

	IEnumerator TrackingDelay()
	{
		if (isTrackingMove) yield break;

		isTrackingMove = true;

		yield return new WaitForSeconds (0.5f);

		isTrackingMove = false;
	}


	IEnumerator BackMovingDelay()
	{
		if (isBackMoving) yield break;

		isBackMoving = true;

		yield return new WaitForSeconds (Random.Range(1.5f, 2f));

		isBackMoving = false;
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

                yield return new WaitForSeconds(Random.Range(3, 8));

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

            //TODO AI Patch
            
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

        isDamage = true;
    }

    void DamageEffect()
    {
        if (isDamage)
        {
            mySprite.color = new Color(255f, 0f, 0f);
        }
        else
        {
            mySprite.color = Color.Lerp(mySprite.color, myColor, flashSpeed * Time.deltaTime);
        }

        isDamage = false;

    }

    public void SetStrength(float strength)
    {
        currentHealth = (int)strength * 10 + currentHealth;

        max_health = currentHealth;
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
