﻿//공을 움직이는 코드
using UnityEngine;
using System.Collections;

public class Ball_Moter : MonoBehaviour
{
	public float limitPadding;

    public float moveSpeed = 10.0f;
    public float drag = 0.5f;
    public Vector3 MoveVector;
    public VirtualJoystick joystick;
	private LimitArea limitArea;


    private Rigidbody2D thisRigidbody;

    void Start()
    {
        thisRigidbody = gameObject.AddComponent<Rigidbody2D>();
        thisRigidbody = GetComponent<Rigidbody2D>();
        thisRigidbody.drag = drag;
    }

    private void Update()
    {
        MoveVector = PoolInput();

        Move();

        Ontouch();


    }

    private void Move()
    {
        thisRigidbody.velocity = MoveVector * moveSpeed;

        //transform.position += limitArea.Clamp(MoveVector + transform.position * moveSpeed * Time.deltaTime);
        //rigidbody.position = limitArea.Clamp(transform.position + movePos); 
    }

    public void Ontouch()
    {
        if (Input.GetMouseButtonUp(0) == false) return;

        Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 target;
        target.x = mpos.x - transform.position.x;
        target.y = mpos.y - transform.position.y;
        float angle = -1 * Mathf.Rad2Deg * Mathf.Atan2(target.x, target.y) + 90;

        transform.eulerAngles = new Vector3(0, 0, angle + 180.0f);

        //transform.Find("Sword_test");
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