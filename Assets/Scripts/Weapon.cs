using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public enum Owner{
        Player,
        Enermy
    }
    int i = 0;
    public Owner owner;

    public int damage;
	public float attackDelay;

    public bool isEpic;
    public bool isSpeacial;

    public Animator weaponAni;

    public PolygonCollider2D myColider;

    private Collider2D[] allCollider;

    public bool _isAttacking;

	public bool _isBlocking;

    // Use this for initialization
    void Start()
    {
        allCollider = FindObjectsOfType<Collider2D>();
        PolygonCollider2D myColider = GetComponent<PolygonCollider2D>();
        weaponAni = GetComponent<Animator>();
        StartCoroutine(ReloadAllCollider());
        StartCoroutine(CheackOwner());

    }
        // Update is called once per frame
    void Update () {
        
    }

    IEnumerator ReloadAllCollider()
    {
        while (i < allCollider.Length)
        {
            yield return null;
            Physics2D.IgnoreCollision(myColider, allCollider[i], false);
            i++;
        }
    }

    IEnumerator CheackOwner()
    {
        while(true)
        {
            yield return null;
            if (transform.parent.CompareTag("Player"))
                owner = Owner.Player;
            else if (transform.parent.CompareTag("Enermy"))
                owner = Owner.Enermy;
        }
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

		yield return new WaitForSeconds (0.000001f);
	}

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (_isAttacking) {
            Physics2D.IgnoreCollision(myColider, hit, true);

            if (owner == Owner.Enermy && hit.CompareTag("Player"))
                Debug.Log("Enermy is hit on Player");
            else if (owner == Owner.Player && hit.CompareTag("Enermy"))
                Debug.Log("Player is hit on Enermy");
        } else if (_isBlocking) {
			
		}
        
    }

    private void OnTriggerExit2D(Collider2D hit)
    {
        Physics2D.IgnoreCollision(myColider, hit, false);
    }
}
