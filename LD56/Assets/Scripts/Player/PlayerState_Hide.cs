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
		base.EnterState(stateMachineContext);
	}

	public override void ExitState(StateMachine stateMachineContext)
	{
		m_playerInputSystems.GetComponent<EdgeCollider2D>().excludeLayers = 0;
		base.ExitState(stateMachineContext);
	}
}
