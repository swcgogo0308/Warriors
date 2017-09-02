using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public int damage;
	public float attackDelay;

    public bool isEpic;
    public bool isSpeacial;

    public Animator weaponAni;

    public PolygonCollider2D myColider;

    public bool _isAttacking;

	public bool _isBlocking;

    // Use this for initialization
    void Start () {
        PolygonCollider2D myColider = GetComponent<PolygonCollider2D>();
        weaponAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {

	}

    public void Attack()
    {
        StartCoroutine(Attacking());
    }

	public void Shild(bool touchDown)
    {
		//if (_isAttacking) return;
		StartCoroutine (Blocking (touchDown));
        //TODO Guard
    }

    IEnumerator Attacking()
    {
        _isAttacking = true;
		 
        weaponAni.SetBool("isAttacking", true);
        yield return new WaitForSeconds(1f);
        weaponAni.SetBool("isAttacking", false);

		yield return new WaitForSeconds(attackDelay);

        _isAttacking = false;
    }

	IEnumerator Blocking(bool touchDown)
	{
		weaponAni.SetBool ("isBlocking", touchDown);

		yield return new WaitForSeconds (0.005f);
	}

    private void OnTriggerEnter2D(Collider2D hit)
    {
		if (_isAttacking) {

			if (hit.tag == "Weapon") {
				//TODO Parring
			}
		} else if (_isBlocking) {
			
		}
    }
}
