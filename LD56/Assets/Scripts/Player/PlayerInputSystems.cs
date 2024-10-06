using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class PlayerInputSystems : MonoBehaviour
{
	private Vector2 moveInput;
	public float moveSpeed;
	public float jumpHeight = 15;
	public bool allowMoveVertical;
	public bool allowJump;
	public bool m_pickUp;
	private bool isJumping;
	private float jumpOriginalHeight;
	private Player player;
	private PickableObject currentPickedObject;
	PlayerStateMachine m_playerStateMachine;
	Rigidbody2D m_rigidbody2D;
	Animator animator;

	private void Start()
    {
		m_pickUp = false;
		player = this.GetComponent<Player>();
		m_playerStateMachine = PlayerStateMachine.Instance;
		m_rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

    public void OnMove(InputValue value)
	{
		moveInput = value.Get<Vector2>();
	}

	public void OnInteract()
	{
		GetComponent<Interact>().InteractWithObject();
	}

	public void OnJump()
	{
		if (allowJump && !isJumping)
		{
			isJumping = true;
			animator.SetTrigger("Jumping");
			jumpOriginalHeight = player.transform.position.y;
			//CameraManager.Instance.isFollowing = false;
			m_rigidbody2D.velocity += new Vector2(0, jumpHeight);
		}
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
			GetComponent<Interact>().PutDownObject(currentPickedObject);
			m_playerStateMachine.ChangeToDefaultState();
			m_pickUp = false;
		}
	}

	

	private void Update()
	{
		if ((isJumping && player.transform.position.y <= jumpOriginalHeight && m_rigidbody2D.velocity.y < 0) || !allowJump)
		{
			isJumping = false;
			//CameraManager.Instance.isFollowing = true;
		}
		// Use the moveInput to move the player
		Vector2 move;
		if (allowMoveVertical)
		{
			move = new Vector2(moveInput.x, moveInput.y);
			m_rigidbody2D.velocity = move * moveSpeed;
		}
		else
		{
			move = new Vector2(moveInput.x, 0);
			m_rigidbody2D.velocity = new Vector2(move.x * moveSpeed, m_rigidbody2D.velocity.y);
		}
	}

	public void ToggleGravity(bool enable)
	{
		if (enable)
		{
			m_rigidbody2D.gravityScale = Constant.gravityScale;
		}
		else
		{
			m_rigidbody2D.gravityScale = 0;
		}
	}
}
