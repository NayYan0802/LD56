using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	public Transform pickOffset;
    PlayerStateMachine m_playerStateMachine;
	private float inspectTime = 0;
	[SerializeField]private float inspectMax = 5;

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

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Inspector")
		{
			inspectTime += Time.deltaTime;
			if (inspectTime >= inspectMax)
			{ 
				//player freeze
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Ladder")
		{
			m_playerStateMachine.ChangeToDefaultState();
		}
		else if (collision.tag == "Inspector")
		{
			inspectTime = 0;
		}
	}
}
