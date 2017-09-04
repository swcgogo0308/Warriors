using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Transform playerTransform;

    public Transform rayTransform;

    Vector3 direction;

    public float moveSpeed;

    public float rotateSpeed;

    public float range;

    public bool isRange;

    [Header("Weapon")]
    public Weapon myWeapon;
    // Use this for initialization
    void Start () {
        if (myWeapon == null)
        {
            myWeapon = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Weapon>();
        }

        StartCoroutine(CheckFoward());
    }
	
	// Update is called once per frame
	void Update () {

        Move();
	}

    private void Move()
    {
        if (myWeapon._isAttacking || isRange) return;
        direction = playerTransform.position - transform.position;
        direction.z = 0;
        direction.Normalize();

        Vector3 movePos = transform.position + (direction * moveSpeed * Time.deltaTime);
        transform.position = movePos;
    }

    private IEnumerator CheckFoward()
    {
        while (true)
        {
            yield return null;

            Ray2D ray = new Ray2D(rayTransform.position, transform.right);

            RaycastHit2D rayHit = Physics2D.Raycast(ray.origin, ray.direction, range);

            if (rayHit.collider == null || rayHit.collider.tag != "Player")
            {
                isRange = false;
                continue;
            }

            isRange = true;
            Attack();

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Attack()
    {
        if (myWeapon._isAttacking) return;

        myWeapon.Attack();
    }
}
