using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Hide : PlayerState_Base
{
	public override void EnterState(StateMachine stateMachineContext)
	{
		LayerMask mask = LayerMask.GetMask("Inspector");
		m_playerInputSystems.GetComponent<EdgeCollider2D>().excludeLayers = mask;
		m_playerInputSystems.moveSpeed = 0f;

		Color color = m_playerInputSystems.GetComponent<SpriteRenderer>().color;
		color.a = 0.5f;
		m_playerInputSystems.GetComponent<SpriteRenderer>().color = color;
		base.EnterState(stateMachineContext);
	}

	public override void ExitState(StateMachine stateMachineContext)
	{
		Color color = m_playerInputSystems.GetComponent<SpriteRenderer>().color;
		color.a = 1f;
		m_playerInputSystems.GetComponent<SpriteRenderer>().color = color;

		m_playerInputSystems.GetComponent<EdgeCollider2D>().excludeLayers = 0;
		base.ExitState(stateMachineContext);
	}
}
