using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {

    public enum Owner{
        Player,
        Enemy,
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
    public bool _isButtonActive;
    public bool isOnButton = false;


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
		StartCoroutine(ReloadAllCollider());
        StartCoroutine(CheackFallen());
        
    }
        // Update is called once per frame
    void Update () {
		
    }

	void GetWeapon(bool isButtonActive)
    {
        if (isButtonActive)
        {
            isOnButton = true;
        }
    }

    IEnumerator CheackFallen()
    {        
        while(true)
        {
            yield return null;
            if (owner == Owner.Fallen)
            {
				getButton.onClick.AddListener(() => GetWeapon(_isButtonActive));
               	yield return DestoryCount();
            }
			else
				isFallen = false;
        }
    }

    IEnumerator DestoryCount()
    {
        if (isFallen) yield break;

        isFallen = true;

        yield return new WaitForSeconds(5f);

		if (isFallen)
			Destroy (transform.parent.gameObject);

    }

    

    IEnumerator ReloadAllCollider()
    {

		allCollider = FindObjectsOfType<Collider2D> ();

		for (int i = 0; i < allCollider.Length; i++) 
		{
			yield return null;

			for (int j = 0; j <= i; j++) 
			{
				if (allCollider [j] == allCollider [i])
					continue;
				
				Physics2D.IgnoreCollision (myColider, allCollider [i], false);
			}

		}

		for(int i = 0; i < allCollider.Length; i++)
			Debug.Log(allCollider[i]);

        yield return CheackOwner();
    }

    IEnumerator CheackOwner()
    {
        Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();

        while (true)
        {
            yield return null;

			if (transform.parent.CompareTag ("Player")) {
                Physics2D.IgnoreCollision(myColider, playerCollider, false);
                owner = Owner.Player;
			} else if (transform.parent.CompareTag ("Enemy")) {
                Physics2D.IgnoreCollision(myColider, playerCollider, false);
                owner = Owner.Enemy;
			}
			else if (transform.parent.CompareTag ("Fallen")) {
				Physics2D.IgnoreCollision (myColider, playerCollider, true);
                owner = Owner.Fallen;
			}
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
		if (isFallen) {
			
			//Physics2D.IgnoreCollision (myColider, hit, true);

			_isButtonActive = true;


			if (!isOnButton)
				return;

			Transform playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;

			transform.parent = playerTransform;

			PlayerMove playerMoveScript = FindObjectOfType<PlayerMove>();

			playerMoveScript.WeaponBreak();

			owner = Owner.Player;

            isOnButton = false;
		}

        else if (_isAttacking)
        {
            _isButtonActive = false;

            Physics2D.IgnoreCollision(myColider, hit, true);

            if (owner == Owner.Enemy && hit.CompareTag("Player"))
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

    private void OnTriggerStay2D(Collider2D hit)
    {
        if (isFallen)
        {
            _isButtonActive = true;
        }
        else
            _isButtonActive = false;

    }

    private void OnTriggerExit2D(Collider2D hit)
    {
        if(isFallen)
        {
            Debug.Log("Exit");
            _isButtonActive = false;
            return;
        }
		    
	    Physics2D.IgnoreCollision(myColider, hit, false);
    }
}
