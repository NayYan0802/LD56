using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Hide : PlayerState_Base
{
	public override void EnterState(StateMachine stateMachineContext)
	{
		m_playerInputSystems.moveSpeed = 0f;
		base.EnterState(stateMachineContext);
	}
}
