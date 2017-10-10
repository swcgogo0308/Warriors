//공을 움직이는 코드
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    public Slider healthBarSlider;

    public PlayerHealth playerHealth;

    [Header ("Weapon")]
    public Weapon myWeapon;

    [Header ("LimitPadding")]
	public float limitPadding;

    [Header ("Movement")]
    public float moveSpeed = 10.0f;

    [Header ("Joystick")]
    public Joystick joystick;

	private LimitArea limitArea;
    private Vector3 MoveVector;


    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        SetupLimitArea();

        foreach(Transform child in transform)
        {
            if (child.CompareTag("Weapon"))
                myWeapon = child.GetComponent<Weapon>();
        }

    }

    private void Update()
    {
        Move();

        OnMouseButton();

        OnTouch();
    }

    private void Move()
    {
        MoveVector = joystick.GetInputVector();

        transform.position = limitArea.Clamp(transform.position + MoveVector * moveSpeed * Time.deltaTime);
    }

    private void OnMouseButton()
    {
		if (joystick.isDrag || myWeapon == null) return;

            if (Input.GetMouseButtonDown (0) && !myWeapon._isAttacking) {
			t_Rotate ();
			Attack ();
		}

		/*else if (Input.GetMouseButton(0)) {
			if(!myWeapon._isAttacking)
				Rotate ();
			myWeapon.Shild (true);
		}

		else if (Input.GetMouseButtonUp (0)) {

			myWeapon.Shild (false);
		}*/
	}

    private void OnTouch()
    {
        if (joystick.isDrag || myWeapon == null) return;

        if (Input.GetTouch(0).phase == TouchPhase.Began && !myWeapon._isAttacking)
        {
            t_Rotate();
            Attack();
        }
    }

    void Attack()
    {
        myWeapon.Attack(playerHealth.isDead);
    }

    void t_Rotate()
    {
        Vector3 mpos = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        Vector3 target;
        target.x = mpos.x - transform.position.x;
        target.y = mpos.y - transform.position.y;
        float angle = -1 * Mathf.Rad2Deg * Mathf.Atan2(target.x, target.y) + 90;
        transform.eulerAngles = new Vector3(0, 0, angle + 180.0f);
    }

    void m_Rotate()
    {
        Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 target;
        target.x = mpos.x - transform.position.x;
        target.y = mpos.y - transform.position.y;
        float angle = -1 * Mathf.Rad2Deg * Mathf.Atan2(target.x, target.y) + 90;
        transform.eulerAngles = new Vector3(0, 0, angle + 180.0f);
        /*
        Vector3 mpos = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        Vector3 target;
        target.x = mpos.x - transform.position.x;
        target.y = mpos.y - transform.position.y;
        float angle = -1 * Mathf.Rad2Deg * Mathf.Atan2(target.x, target.y) + 90;
        transform.eulerAngles = new Vector3(0, 0, angle + 180.0f);
        */
    }

	private void SetupLimitArea()
	{
		limitArea = MapManager.LimitArea.AddMargin(limitPadding);
	}

	public void WeaponBreak()
	{
		Destroy (myWeapon.gameObject);

		foreach (Transform child in transform)
		{
			if (child.CompareTag("Weapon"))
				myWeapon = child.GetComponent<Weapon>();
		}
	}
}