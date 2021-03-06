﻿//공을 움직이는 코드
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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
			Rotate ();
			Attack ();
		}
	}

    private void OnTouch()
    {
		if (myWeapon == null && IsPointerOverUIObject()) return;

        if (Input.GetTouch(0).phase == TouchPhase.Began && !myWeapon._isAttacking)
        {
            Rotate();
            Attack();
        }
    }

    void Attack()
    {
        myWeapon.Attack(playerHealth.isDead);
    }
    
    void Rotate()
    {
        #if UNITY_EDITOR
            Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 target;
            target.x = mpos.x - transform.position.x;
            target.y = mpos.y - transform.position.y;
            float angle = -1 * Mathf.Rad2Deg * Mathf.Atan2(target.x, target.y) + 90;
            transform.eulerAngles = new Vector3(0, 0, angle + 180.0f);
        #elif UNITY_ANDROID
            if(Input.touchCount <= 0) return;
            Vector2 pos = Input.GetTouch(0).position;
            Vector3 theTouch = new Vector3(pos.x, pos.y, 0.0f);   
            Vector3 mpos = Camera.main.ScreenToWorldPoint(theTouch);
            Vector3 target;
            target.x = mpos.x - transform.position.x;
            target.y = mpos.y - transform.position.y;
            float angle = -1 * Mathf.Rad2Deg * Mathf.Atan2(target.x, target.y) + 90;
            transform.eulerAngles = new Vector3(0, 0, angle + 180.0f);
        #endif
    }

	private bool IsPointerOverUIObject() //UI touch check
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);

		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
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