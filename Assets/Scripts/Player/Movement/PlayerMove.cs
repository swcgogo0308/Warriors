//공을 움직이는 코드
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    public Slider healthBarSlider;

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
        SetupLimitArea();

        StartCoroutine(GetWeapon());
    }

    private void Update()
    {
        Move();

        Ontouch();
    }

    IEnumerator GetWeapon()
    {
        if (myWeapon == null)
        {
            myWeapon = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Weapon>();
            while (!myWeapon.transform.parent.CompareTag("Player"))
            {
                yield return null;
                myWeapon = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Weapon>();
            }
            
        }
    }

    private void Move()
    {
        MoveVector = joystick.GetInputVector();

        transform.position = limitArea.Clamp(transform.position + MoveVector * moveSpeed * Time.deltaTime);
    }

    private void Ontouch()
    {
		if (joystick.isDrag || myWeapon == null) return;

		if (Input.GetMouseButtonDown (0) && !myWeapon._isAttacking) {
			Rotate ();
			myWeapon.Attack ();
		}

		else if (Input.GetMouseButton(0)) {
			if(!myWeapon._isAttacking)
				Rotate ();
			myWeapon.Shild (true);
		}

		else if (Input.GetMouseButtonUp (0)) {

			myWeapon.Shild (false);
		}

			
	}

    void Rotate()
    {
        Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 target;
        target.x = mpos.x - transform.position.x;
        target.y = mpos.y - transform.position.y;
        float angle = -1 * Mathf.Rad2Deg * Mathf.Atan2(target.x, target.y) + 90;
        transform.eulerAngles = new Vector3(0, 0, angle + 180.0f);
    }

	private void SetupLimitArea()
	{
		limitArea = MapManager.LimitArea.AddMargin(limitPadding);
	}
}