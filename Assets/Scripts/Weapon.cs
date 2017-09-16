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
		StartCoroutine (CheackOwner ());
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
               // yield return DestoryCount();
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

		while (true) 
		{
			allCollider = FindObjectsOfType<Collider2D> ();

			if (owner != Owner.Fallen) 
			{
				while (i < allCollider.Length) 
				{
					yield return null;
					Physics2D.IgnoreCollision (myColider, allCollider [i], false);
					i++;
				}
			} 

			else if (owner == Owner.Fallen) 
			{
				while (i < allCollider.Length) 
				{
					yield return null;
					Physics2D.IgnoreCollision (myColider, allCollider [i], true);
					i++;
				}
			}

			//i = 0;
		}
    }

    IEnumerator CheackOwner()
    {
        while(true)
        {
            yield return null;

			if (transform.parent.CompareTag ("Player")) {
				owner = Owner.Player;
			} else if (transform.parent.CompareTag ("Enemy")) {
				owner = Owner.Enemy;
			}
			else if (transform.parent.CompareTag ("Fallen")) {
				Collider2D playerCollider = GameObject.FindGameObjectWithTag ("Player").GetComponent<Collider2D>();
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
		Debug.Log (hit.tag);
		if (owner == Owner.Fallen && hit.CompareTag("Player")) {
			
			//Physics2D.IgnoreCollision (myColider, hit, true);

			Debug.Log("Enter");

			_isButtonActive = true;

			if (!isOnButton)
				return;

			Transform playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;

			transform.parent = playerTransform;

			PlayerMove playerMoveScript = FindObjectOfType<PlayerMove>();

			playerMoveScript.WeaponBreak();

			owner = Owner.Player;

		}

        if (_isAttacking)
        {
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
		if (owner == Owner.Fallen && hit.CompareTag("Player")) {
			Debug.Log ("Stay");
			_isButtonActive = true;
		}
    }

    private void OnTriggerExit2D(Collider2D hit)
    {
		if (owner == Owner.Fallen && hit.CompareTag ("Player")) {
			_isButtonActive = false;
		}
		if(owner != Owner.Fallen) 
			Physics2D.IgnoreCollision(myColider, hit, false);

        
    }
}
