using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Base : StateMachine.State
{
    protected GameObject self => owner.gameObject;
    protected PlayerStateMachine playerStateMachine;

    [SerializeField] protected PlayerInputSystems m_playerInputSystems;

    public override void InitState(StateMachine context)
    {
        base.InitState(context);

        playerStateMachine = self.GetComponent<PlayerStateMachine>();
    }
}
