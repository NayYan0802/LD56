using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class PlayerInputSystems : MonoBehaviour
{
	private Vector2 moveInput;
	public float moveSpeed;
	public bool allowMoveVertical;
	public bool m_pickUp;
	private Player player;
	private PickableObject currentPickedObject;
	PlayerStateMachine m_playerStateMachine;

	private void Start()
    {
		m_pickUp = false;
		player = this.GetComponent<Player>();
		m_playerStateMachine = PlayerStateMachine.Instance;

	}

    public void OnMove(InputValue value)
	{
		moveInput = value.Get<Vector2>();
	}

	public void OnInteract()
	{
		GetComponent<Interact>().InteractWithObject();
	}

	public void OnPick()
	{
		if (!m_pickUp)
		{
			currentPickedObject = GetComponent<Interact>().InteractWithPickableObject(player);
			m_playerStateMachine.ChangeToPickUpState();
			m_pickUp = true;
		}
		else
		{
			//put down item
			GetComponent<Interact>().PutDownObject();
			m_playerStateMachine.ChangeToDefaultState();
			m_pickUp = false;
		}
	}

	private void Update()
	{
		// Use the moveInput to move the player
		Vector3 move;
		if (allowMoveVertical)
		{
			move = new Vector3(moveInput.x, moveInput.y, 0);
		}
		else
		{
			move = new Vector3(moveInput.x, 0, 0);
		}
		GetComponent<Rigidbody2D>().velocity = move * moveSpeed;
	}

	public void ToggleGravity(bool enable)
	{
		if (enable)
		{
			GetComponent<Rigidbody2D>().gravityScale = Constant.gravityScale;
		}
		else
		{
			GetComponent<Rigidbody2D>().gravityScale = 0;
		}
	}
}
