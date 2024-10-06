using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerState_PickUp : PlayerState_Base
{
	[SerializeField, BoxGroup("Setting")] float m_pickUpSpeed;
	public override void EnterState(StateMachine stateMachineContext)
	{
		m_playerInputSystems.allowJump = true;
		m_playerInputSystems.moveSpeed = m_pickUpSpeed;
		base.EnterState(stateMachineContext);
	}

	public override void ExitState(StateMachine stateMachineContext)
	{
		m_playerInputSystems.allowJump = false;
		base.ExitState(stateMachineContext);
	}
}
