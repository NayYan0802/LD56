using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerState_Default : PlayerState_Base
{
	[SerializeField, BoxGroup("Setting")] float m_defaultSpeed;


	public override void EnterState(StateMachine stateMachineContext)
	{
		m_playerInputSystems.moveSpeed = m_defaultSpeed;
		base.EnterState(stateMachineContext);
	}
}
