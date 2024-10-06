using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	public Transform pickOffset;
    PlayerStateMachine m_playerStateMachine;

	private void Start()
	{
		GetComponent<Rigidbody2D>().gravityScale = Constant.gravityScale;
		m_playerStateMachine = PlayerStateMachine.Instance;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Ladder")
		{
			m_playerStateMachine.ChangeToLadderState();
		}		
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Ladder")
		{
			m_playerStateMachine.ChangeToDefaultState();
		}		
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		
	}
}
