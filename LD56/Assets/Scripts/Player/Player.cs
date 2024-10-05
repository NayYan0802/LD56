using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerStateMachine m_playerStateMachine;

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
}
