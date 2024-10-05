using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class PlayerInputSystems : MonoBehaviour
{
	private Vector2 moveInput;
	public float moveSpeed;

	public void OnMove(InputValue value)
	{
		moveInput = value.Get<Vector2>();
	}

	private void Update()
	{
		// Use the moveInput to move the player
		Vector3 move = new Vector3(moveInput.x, 0, 0);
		transform.Translate(move * moveSpeed * Time.deltaTime);
	}
}
