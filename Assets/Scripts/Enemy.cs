using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Transform playerTransform;

    public Transform rayTransform;

    public Animator anim;

    Vector3 direction;

    public int weaponFallenProbabile;

    public float moveSpeed;

    public float rotateSpeed;

    public float range;

    public bool isRange;

    public int max_health;

    private int health;

    private int currentHealth;

    bool isAttack;

    public bool isDead;

    [Header("Weapon")]
    public Weapon myWeapon;
    // Use this for initialization

    private void Awake()
    {
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

        StartCoroutine(CheckFoward());
    }
	
	// Update is called once per frame
	void Update () {
        if (isDead) return;
        Move();
	}

    private void Move()
    {
        if (myWeapon._isAttacking || isRange) return;
        direction = playerTransform.position - transform.position;
        direction.z = 0;
        direction.Normalize();

        Rotate(false);

        Vector3 movePos = transform.position + (direction * moveSpeed * Time.deltaTime);
        transform.position = movePos;
    }

    void Rotate(bool isAttakck)
    {
        float angle = -1 * Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.y) + 90;

        if(!isAttack)
            transform.eulerAngles = new Vector3(0, 0, angle + 180.0f);
        else
            transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private IEnumerator CheckFoward()
    {
        while (true)
        {
            yield return null;

            Ray2D ray = new Ray2D(rayTransform.position, -transform.right);

            RaycastHit2D rayHit = Physics2D.Raycast(ray.origin, ray.direction, range);

            if (rayHit.collider == null || rayHit.collider.tag != "Player")
            {
                isRange = false;
                continue;
            }

            isRange = true;

            Invoke("Attack", myWeapon.attackDelay);

            yield return new WaitForSeconds(1f);
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
        currentHealth = (int)strength * 10 + currentHealth;
    }

    void Death()
    {
        isDead = true;
        StartCoroutine(DeathEffect());
    }

    IEnumerator DeathEffect()
    {
        anim.SetBool("isDead", true);


        yield return new WaitForSeconds(1.5f);

        if (Random.Range(1, 100) <= weaponFallenProbabile)
        {
            Transform fallenWeaponStorage = GameObject.FindGameObjectWithTag("Fallen").transform;
            fallenWeaponStorage.position = this.transform.position;

            myWeapon.gameObject.transform.parent = fallenWeaponStorage;
        }

        Destroy(gameObject);
    }
}
