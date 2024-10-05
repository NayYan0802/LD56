using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public static PlayerStateMachine Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    private void Start()
    {
        //default state
        ChangeToDefaultState();
    }

    public void ChangeToDefaultState() { SwitchToState(GetStateByType<PlayerState_Default>()); }
    public void ChangeToHideState() { SwitchToState(GetStateByType<PlayerState_Hide>()); }
    public void ChangeToPickUpState() { SwitchToState(GetStateByType<PlayerState_PickUp>()); }
    public void ChangeToLadderState() { SwitchToState(GetStateByType<PlayerState_Ladder>()); }
}
