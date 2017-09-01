using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public int damage;
    public float weaponDelay;

    public bool isEpic;
    public bool isSpeacial;

    public Animator attack;

    public PolygonCollider2D myColider;

    public bool _isAttacking;

    // Use this for initialization
    void Start () {
        PolygonCollider2D myColider = GetComponent<PolygonCollider2D>();
        attack = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {

	}

    public void Attack()
    {
        StartCoroutine(Attacking());
    }

    public void Shild()
    {
        if (_isAttacking) return;

        //TODO Guard
    }

    IEnumerator Attacking()
    {
        _isAttacking = true;
        attack.SetBool("isAttacking", true);
        yield return new WaitForSeconds(1f);
        attack.SetBool("isAttacking", false);
        yield return new WaitForSeconds(weaponDelay);

        _isAttacking = false;

    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (_isAttacking == false) return;

        if (hit.tag == "Weapon")
        {
            //TODO Parring
        }
    }
}
