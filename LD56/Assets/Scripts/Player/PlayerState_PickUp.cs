using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerState_PickUp : PlayerState_Base
{
	[SerializeField, BoxGroup("Setting")] float m_pickUpSpeed;
	public override void EnterState(StateMachine stateMachineContext)
	{
		m_playerInputSystems.moveSpeed = m_pickUpSpeed;
		base.EnterState(stateMachineContext);
	}
}
