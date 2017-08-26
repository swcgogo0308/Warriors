using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMove : MonoBehaviour
{
	public Joystick joystick;

	[Header("Move limit option")]
	public float limitPadding;

	new private Rigidbody2D rigidbody;
	private Player.MoveData data;
	private LimitArea limitArea;
	private Vector3 inputVector;

	private float moveSpeed;
	private bool isPause;

	// Use this for initialization
	void Start()
	{
		PauseButton pauseButton = Manager.Get<PauseButton>();
		pauseButton.OnPlayEventHandler += OnPlayEvent;
		pauseButton.OnPauseEventHandler += OnPauseEvent;

		AbilityManager ability = Manager.Get<AbilityManager>();
		int energyAbilityLevel = ability.Get(AbilityType.RotateSpeed) - 1;

		rigidbody = GetComponent<Rigidbody2D>();
		data = Manager.Get<Player>().moveData;
		data.SetAbilityLevel(energyAbilityLevel);
		SetupLimitArea();
	}

	public void TryMove(Player player)
	{
		if (isPause) {
			return;
		}

		UpdateInputValue();

		bool canMove = CanMove(player);
		player.animator.SetBool("Move", canMove);

		if (canMove)
		{
			Move(player);
			Rotate(player);
		}
	}

	public void Move(Player player) {
		if (StateEquals(player, typeof(RollingState))) {
			moveSpeed = data.moveSpeed * 1.4f;
		} else {
			moveSpeed = data.moveSpeed;
		}

		Vector3 movePos = transform.right * inputVector.magnitude
		                           * moveSpeed * Time.fixedDeltaTime;
		rigidbody.position = limitArea.Clamp(transform.position + movePos);
	}

	public void Rotate(Player player) {
		float angle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;

		Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = 
			Quaternion.RotateTowards(transform.rotation, newRotation, data.rotateSpeed);
	}

	private bool StateEquals(Player player, Type state) {
		return player.State.GetType() == state;
	}

	private void UpdateInputValue() { 
		inputVector = joystick.GetInputVector();
	}

	private bool CanMove(Player player)
	{
		if (StateEquals(player, typeof(ChargeState))) {
			return false;
		}

		return inputVector.sqrMagnitude > 0;
	}

	private void SetupLimitArea()
	{
		limitArea = MapManager.LimitArea.AddMargin(limitPadding);
	}

	private void OnPauseEvent()
	{
		isPause = true;
	}

	private void OnPlayEvent() {
		isPause = false;
	}
}
