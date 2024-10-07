using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerState_Ladder : PlayerState_Base
{
	[SerializeField, BoxGroup("Setting")] float m_ladderSpeed;
	public override void EnterState(StateMachine stateMachineContext)
	{
		m_playerInputSystems.allowJump = true;
		LayerMask mask = LayerMask.GetMask("Default");
		m_playerInputSystems.GetComponent<EdgeCollider2D>().excludeLayers = mask;
		m_playerInputSystems.moveSpeed = m_ladderSpeed;
		m_playerInputSystems.allowMoveVertical = true;
		m_playerInputSystems.ToggleGravity(false);
		base.EnterState(stateMachineContext);
	}

	public override void ExitState(StateMachine stateMachineContext)
	{
		m_playerInputSystems.allowJump = false;
		m_playerInputSystems.GetComponent<Animator>().SetBool("Climbing", false);
		LayerMask mask = 0;
		m_playerInputSystems.GetComponent<EdgeCollider2D>().excludeLayers = mask;
		m_playerInputSystems.gameObject.layer = 9;
		m_playerInputSystems.ToggleGravity(true);
		m_playerInputSystems.allowMoveVertical = false;
		base.ExitState(stateMachineContext);
	}
}
