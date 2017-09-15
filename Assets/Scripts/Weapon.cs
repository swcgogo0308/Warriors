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

    public enum Grade
    {
        Normal,
        Epic,
        Speacial
    }
    public Owner owner;

    public Grade weaponGrade = Grade.Normal;

    public PlayerHealth playerHealth;

    public Button getButton;

    public Text getButtonText;

    public int damage;
	public float attackDelay;

    public bool isEpic;
    public bool isSpeacial;
    private bool isFallen;
    private bool isTakeDamage;
    private bool isButtonActive;
    public static bool isOnButton;


    public Animator weaponAni;

    public PolygonCollider2D myColider;

    private Collider2D[] allCollider;

    private Enemy[] enemysObject;

    public bool _isAttacking;

	public bool _isBlocking;



    // Use this for initialization
    void Start()
    {
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        allCollider = FindObjectsOfType<Collider2D>();
        PolygonCollider2D myColider = GetComponent<PolygonCollider2D>();
        weaponAni = GetComponent<Animator>();
        getButton = GameObject.FindGameObjectWithTag("GetButton").GetComponent<Button>();
        StartCoroutine(CheackOwner());
        StartCoroutine(CheackFallen());
        StartCoroutine(ReloadAllCollider());
    }
        // Update is called once per frame
    void Update () {
        if (isOnButton && owner == Owner.Player)
        {
            Destroy(gameObject);
        }
    }

    void GetWeapon()
    {
        if (isButtonActive)
        {
            isOnButton = true;
            isButtonActive = false;
        }
    }

    IEnumerator CheackFallen()
    {        
        while(true)
        {
            yield return null;
            if (owner == Owner.Fallen)
            {
                //getButton.onClick.AddListener(() => GetWeapon());
                yield return DestoryCount();
            }
        }
    }

    IEnumerator DestoryCount()
    {
        if (isFallen) yield break;

        isFallen = true;

        yield return new WaitForSeconds(5f);

		if (owner == Owner.Fallen)
			Destroy (transform.parent.gameObject);

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
                isButtonActive = true;
            }

            getButton.onClick.AddListener(() => { GetWeapon(); });

            if (!isOnButton) return;

            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            transform.parent = playerTransform;

        }

        if (_isAttacking)
        {
            Physics2D.IgnoreCollision(myColider, hit, true);

            if (owner == Owner.Enermy && hit.CompareTag("Player"))
            {
                if (isTakeDamage) return;
                playerHealth.TakeDamage(damage);
                StartCoroutine(DamageDelay());
            }
            else if (owner == Owner.Player && hit.CompareTag("Enemy"))
            {
                enemysObject = FindObjectsOfType<Enemy>();

                for (int i = 0; i < enemysObject.Length; i ++)
                {

                    if (enemysObject[i].transform == hit.transform && !enemysObject[i].isOnDamage)
                    {
                        StartCoroutine(CheackEnemyCount(hit));
                        break;
                    }
                }

                
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


    IEnumerator DamageDelay(Enemy enemy)
    {
        if (enemy.isOnDamage) yield break;

        enemy.isOnDamage = true;
        isTakeDamage = true;
        yield return new WaitForSeconds(1f);
        isTakeDamage = false;
        enemy.isOnDamage = false;
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
                yield return DamageDelay(enemysObject[i]);
                yield break;
            }
            else if (enemysObject.Length <= 0)
                yield return null;

            i++;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D hit)
    {
        if(isFallen)
            isButtonActive = false;

        Physics2D.IgnoreCollision(myColider, hit, false);

        
    }
}
