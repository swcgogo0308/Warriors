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

    public PlayerHealth playerHealth;

    public int damage;
	public float attackDelay;

    public bool isEpic;
    public bool isSpeacial;
    public bool isFallen;

    public Animator weaponAni;

    public PolygonCollider2D myColider;

    private Collider2D[] allCollider;

    private Enemy[] enemysObject;

    public bool _isAttacking;

	public bool _isBlocking;


    // Use this for initialization
    void Start()
    {
        allCollider = FindObjectsOfType<Collider2D>();
        PolygonCollider2D myColider = GetComponent<PolygonCollider2D>();
        weaponAni = GetComponent<Animator>();
        StartCoroutine(CheackFallen());
        StartCoroutine(ReloadAllCollider());
        if (isFallen) return;
        StartCoroutine(CheackOwner());

    }
        // Update is called once per frame
    void Update () {
        
    }

    IEnumerator CheackFallen()
    {
        if (transform.parent == null)
            isFallen = true;
        yield return null;
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
            else if (transform.parent.CompareTag("Enemy"))
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
        if (isFallen) return;

        if (_isAttacking) {
            Physics2D.IgnoreCollision(myColider, hit, true);

            if (owner == Owner.Enermy && hit.CompareTag("Player"))
                playerHealth.TakeDamage(damage);
            else if (owner == Owner.Player && hit.CompareTag("Enemy"))
                StartCoroutine(CheackEnemyCount(hit));
        } else if (_isBlocking) {
			
		}
    }

    IEnumerator CheackEnemyCount(Collider2D hit)
    {
        int i = 0;
        while (true)
        {
            yield return null;
            enemysObject = FindObjectsOfType<Enemy>();

            if (enemysObject[i].transform == hit.transform)
            {
                enemysObject[i].TakeDamage(damage);
                yield break;
            }

            i++;
        }
    }

    private void OnTriggerExit2D(Collider2D hit)
    {
        Physics2D.IgnoreCollision(myColider, hit, false);
    }
}
