using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    public Dictionary<StateEnum, State> states = new Dictionary<StateEnum, State>();

    public State currentState;
    private IUser owner;

    public FSM(IUser _owner, StateEnum startState, params State[] statesList)
    {
        owner = _owner;

        foreach (State state in statesList)
        {
            state.Init(this);
            states.Add(state.id, state);
        }

        SwitchState(startState);
    }

    public void SwitchState(StateEnum _newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = states[_newState];

        if (currentState != null)
        {
            currentState.OnEnter(owner);
        }
    }

    public void OnUpdate()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }
}
