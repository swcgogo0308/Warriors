//공을 움직이는 코드
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [Header ("Weapon")]
    public Weapon myWeapon;

    [Header ("LimitPadding")]
	public float limitPadding;

    [Header ("Movement")]
    public float moveSpeed = 10.0f;
    public float drag = 0.5f;

    [Header ("Joystick")]
    public VirtualJoystick joystick;

	private LimitArea limitArea;
    private Vector3 MoveVector;

    private Rigidbody2D thisRigidbody;

    void Start()
    {
        thisRigidbody = gameObject.AddComponent<Rigidbody2D>();
        thisRigidbody = GetComponent<Rigidbody2D>();
        thisRigidbody.drag = drag;
    }

    private void LateUpdate()
    {
        MoveVector = PoolInput();

		Rotate_keybord ();

        Move();

        Ontouch();

    }

    private void Move()
    {
        thisRigidbody.velocity = MoveVector * moveSpeed;

        //transform.position += limitArea.Clamp(MoveVector + transform.position * moveSpeed * Time.deltaTime);
        //rigidbody.position = limitArea.Clamp(transform.position + movePos); 
    }

    private void Ontouch()
    {
		if (joystick.isDrag) return;

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

	void Rotate_keybord()
	{
		if (Input.GetKey (KeyCode.W)) {
			transform.eulerAngles = new Vector3 (0, 0, 270);
		}

		else if (Input.GetKey (KeyCode.S)) {
			transform.eulerAngles = new Vector3 (0, 0, 90);
		}

		else if (Input.GetKey (KeyCode.A)) {
			transform.eulerAngles = new Vector3 (0, 0, 0);
		}

		else if (Input.GetKey (KeyCode.D)) {
			transform.eulerAngles = new Vector3 (0, 0, 180);
		}
	}

    private Vector3 PoolInput()
    {
        Vector3 dir = Vector3.zero;

        //dir.x= Input.GetAxis("Horizontal");
        //dir.z= Input.GetAxis("vertical");

        dir.x = joystick.Horizontal();
        dir.y = joystick.Vertical();

        if (dir.magnitude > 1)
            dir.Normalize();

        return dir;
    }


	private void SetupLimitArea()
	{
		limitArea = MapManager.LimitArea.AddMargin(limitPadding);
	}
}