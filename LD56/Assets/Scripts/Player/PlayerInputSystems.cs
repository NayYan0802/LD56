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

    private void Start()
    {
		m_pickUp = false;
		player = this.GetComponent<Player>();
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
			m_pickUp = true;
		}
		else
		{
			//put down item
			GetComponent<Interact>().PutDownObject();
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
		transform.Translate(move * moveSpeed * Time.deltaTime);
	}

	public void ToggleGravity(bool enable)
	{
		if (enable)
		{
			GetComponent<Rigidbody2D>().gravityScale = 1f;
		}
		else
		{
			GetComponent<Rigidbody2D>().gravityScale = 0;
		}
	}
}
