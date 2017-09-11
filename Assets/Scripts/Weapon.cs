using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {

    public enum Owner{
        Player,
        Enermy,
        Fallen
    }
    public Owner owner;

    public PlayerHealth playerHealth;

    public Button getButton;

    public Text getButtonText;

    public int damage;
	public float attackDelay;

    public bool isEpic;
    public bool isSpeacial;
    private bool isFallen;
    private bool isTakeDamage;
    private bool isOnButton;


    public Animator weaponAni;

    public PolygonCollider2D myColider;

    private Collider2D[] allCollider;

    private Enemy[] enemysObject;

    public bool _isAttacking;

	public bool _isBlocking;

	bool _isAttackAni;



    // Use this for initialization
    void Start()
    {
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        allCollider = FindObjectsOfType<Collider2D>();
        PolygonCollider2D myColider = GetComponent<PolygonCollider2D>();
        weaponAni = GetComponent<Animator>();
        StartCoroutine(CheackOwner());
        StartCoroutine(CheackFallen());
        StartCoroutine(ReloadAllCollider());
    }
        // Update is called once per frame
    void Update () {
        
    }

    void GetWeapon()
    {
        if (isOnButton)
        {
            transform.parent = playerHealth.transform;
        }
    }

    IEnumerator CheackFallen()
    {        
        while(true)
        {
            yield return null;
            if (owner == Owner.Fallen)
            {
                getButton.onClick.AddListener(() => GetWeapon());
                yield return DestoryCount();
            }
        }
    }

    IEnumerator DestoryCount()
    {
        if (isFallen) yield break;

        isFallen = true;

        yield return new WaitForSeconds(5f);

        if(owner == Owner.Fallen)
            Destroy(gameObject);

    }

    

    IEnumerator ReloadAllCollider()
    {
        int i = 0;
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
            else if (transform.parent.CompareTag("Fallen"))
                owner = Owner.Fallen;
        }
        
    }

    public void Attack(bool isDead)
    {
        StartCoroutine(Attacking(isDead));
    }

	public void Shild(bool touchDown)
    {
		//if (_isAttacking) return;
		StartCoroutine (Blocking (touchDown));
        //TODO Guard
    }

    IEnumerator Attacking(bool isDead)
    {
        if (_isAttacking) yield break;

        //yield return new WaitForSeconds(attackDelay);

        if (isDead) yield break;

		_isAttacking = true;
        weaponAni.SetBool("isAttacking", true);
        yield return new WaitForSeconds(1f);
        weaponAni.SetBool("isAttacking", false);
        _isAttacking = false;
    }

	IEnumerator Blocking(bool touchDown)
	{
        if (_isAttacking) yield break;

        weaponAni.SetBool ("isBlocking", touchDown);

		yield return new WaitForSeconds (0.000001f);
	}

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (isFallen)
        {
            Physics2D.IgnoreCollision(myColider, hit, true);

            if (hit.CompareTag("Player"))
            {
                isOnButton = true;
            }

        }
        else if (_isAttacking)
        {
            Physics2D.IgnoreCollision(myColider, hit, true);

            if (isTakeDamage) return;

            if (owner == Owner.Enermy && hit.CompareTag("Player"))
            {
                playerHealth.TakeDamage(damage);
                StartCoroutine(DamageDelay());
            }
            else if (owner == Owner.Player && hit.CompareTag("Enemy"))
            {
                StartCoroutine(CheackEnemyCount(hit));
            }
        } else if (_isBlocking) {
			
		}
    }

    IEnumerator DamageDelay()
    {
        isTakeDamage = true;
        yield return new WaitForSeconds(1f);
        isTakeDamage = false;
    }

    IEnumerator CheackEnemyCount(Collider2D hit)
    {
        int i = 0;
        while (true)
        {
            enemysObject = FindObjectsOfType<Enemy>();

            if (enemysObject[i].transform == hit.transform)
            {
                enemysObject[i].TakeDamage(damage);
                yield return DamageDelay();
                yield break;
            }
            else if (enemysObject.Length <= 0)
                yield return null;

            i++;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isFallen)
        {
        }
    }

    private void OnTriggerExit2D(Collider2D hit)
    {
        isOnButton = false;

        if (isFallen)
        {
            getButtonText.text = "False";
        }

        Physics2D.IgnoreCollision(myColider, hit, false);

        
    }
}
