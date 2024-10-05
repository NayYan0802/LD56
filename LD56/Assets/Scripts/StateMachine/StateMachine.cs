using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-50)]
public class StateMachine : SerializedMonoBehaviour
{
    public enum StateActiveness
    {
        inactive,
        active
    }
    [BoxGroup("State")]
    public bool emitStateChangeEvents = false;
    public bool autoRegisterStates = true;
    private bool hasInitialized = false;

    [ShowIf("autoRegisterStates", true), BoxGroup("State")]
    public GameObject rootObjectForStates; //the object we'll scan for states within (including child objects), if autoRegisterStates is true.

    [BoxGroup("State")]
    public bool autoBuildTypeMap = true; //allows use of GetStateByType<>.

    [NonSerialized] public UnityEvent<IState> OnStateChanged = new();

    public bool StateMachineLocked { get; set; } = false;

    public GameObject owner => gameObject;

    protected virtual void Awake()
    {
        if (autoRegisterStates && rootObjectForStates != null)
            FindAndRegisterStates();

        if (autoBuildTypeMap)
            BuildStateMap();

        InitAllStates();

        hasInitialized = true;
    }

    public bool HasInitialized() => hasInitialized;

    //Must be called after all states have been registered, and the state map has been built, as they may check for other states while initializing.
    void InitAllStates()
    {
        foreach (var state in myStates)
        {
            if (state != null)
                state.InitState(this);
        }
    }

    public virtual void ResetState()
    {
        StateMachineLocked = false;
    }

    private void FindAndRegisterStates()
    {
        var stateComponents = rootObjectForStates.GetComponentsInChildren<StateMachine.IState>();
        foreach (var state in stateComponents)
        {
            AddState(state);

#if UNITY_EDITOR
            Debug.Log("Registered state: " + state.GetInternalName());
#endif
        }
    }

    public interface IState
    {
        public void EnterState(StateMachine context);
        public void ExitState(StateMachine context);
        public void TickState(StateMachine context, float deltaTime);
        public void InitState(StateMachine context);
        public string GetInternalName();
    }

    [System.Serializable]
    public class State : MonoBehaviour, IState
    {
        public string internalName;

        [ReadOnly]
        public StateActiveness activeState;

        public StateMachine owner;

        public virtual string GetDisplayName()
        {
            return internalName;
        }

        public virtual void EnterState(StateMachine stateMachineContext)
        {
            activeState = StateActiveness.active;
        }

        public virtual void ExitState(StateMachine stateMachineContext)
        {
            activeState = StateActiveness.inactive;
        }

        public virtual void TickState(StateMachine stateMachineContext, float deltaTime)
        {

        }

        //One-time initialization of the state
        public virtual void InitState(StateMachine context)
        {
            owner = context;
        }

        public virtual string GetInternalName()
        {
            return internalName;
        }
    }

    [BoxGroup("State")]
    [OdinSerialize] public List<IState> myStates = new List<IState>();

    [SerializeField,ReadOnly] private IState activeState;

    private IState previousState;

    public void AddState(IState state)
    {
        if (state == null)
        {
            Debug.LogError("Attempted to add a null state on object " + gameObject.name);
            return;
        }

        myStates?.Add(state);
    }

    private bool ShouldAllowStateChange(IState state)
    {
        if (StateMachineLocked)
        {
            Debug.Log($"Attempted to switch to state {state.GetInternalName()} while the state machine was locked " + gameObject.name);
            return false;
        }
        return true;
    }

    private bool currentlyHandlingEnteringState = false;

    public bool SwitchToState(IState state)
    {
        if (state == null)
        {
            Debug.LogError("Attempted to switch to null state " + gameObject.name);
            return false;
        }        

        if (!ShouldAllowStateChange(state))
            return false;

        currentlyHandlingEnteringState = true;

        if (activeState != null)
        {
            Debug.Log("Exiting state " + activeState.GetInternalName());
            activeState.ExitState(this);
            previousState = activeState;
            activeState = null;
        }
        else
        {
            Debug.Log("There was no active state to switch from. No ExitState() called.");
        }

        activeState = state;

        if (activeState != null)
        {
            activeState.EnterState(this);
        }

        if (emitStateChangeEvents)
        {
            OnStateChanged?.Invoke(state);
        }

        currentlyHandlingEnteringState = false;

        Debug.Log("Finished switching to state " + state.GetInternalName());

        return true;
    }

    public bool SwitchToPreviousState()
    {
        if (previousState == null)
        {
            Debug.Log("There was no previous state to switch.");
            return false;
        }
        else
        {            
            return SwitchToState(previousState);
        }
    }

    public bool SwitchToStateByName(string stateName)
    {
        var state = FindStateByName(stateName);
        if (state != null)
            return SwitchToState(state);

        Debug.LogError("Attempted to switch to state \"" + stateName + "\" but it didn't exist in the state machine of object " + gameObject.name);
        return false;
    }

    private Dictionary<Type, IState> stateMap = new();

    public void BuildStateMap()
    {
        foreach (var state in myStates)
        {
            if (state == null)
                continue;

            var type = state.GetType();
            if (stateMap.ContainsKey(type))
            {
                continue;
            }
            stateMap.Add(type, state);
        }
    }

    public T GetStateByType<T>() where T : class, IState
    {
        IState result = null;
        stateMap.TryGetValue(typeof(T), out result);

        return result as T;
    }

    public virtual IState FindStateByName(string name)
    {
        foreach (var state in myStates)
        {
            if (state != null && state.GetInternalName().Equals(name))
                return state;
        }

        return null;
    }
}
