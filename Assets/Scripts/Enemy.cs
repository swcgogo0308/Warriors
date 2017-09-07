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

    bool isAttack;

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

            if (isRange)
                yield return new WaitForSeconds(2f);
            else
                continue;
            Attack();

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Attack()
    {
        if (myWeapon._isAttacking) return;

        Rotate(true);

        myWeapon.Attack();
    }
}
