using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine : MonoBehaviour
{
    public Stack<State> States { get; set; }

    private void Awake()
    {
        States = new Stack<State>();
    }

    private void Update()
    {
        // Every frame, check if there is an active state...
        if (GetCurrentState() != null)
        {
            // ... if there is: invoke its Active Action
            GetCurrentState().ActiveAction.Invoke();
        }
    }

    // Method for removing state
    public void PopState() // allows to remove state currently on top
    {
        if (GetCurrentState() != null)
        {
            //exit previous state
            GetCurrentState().OnExit();
            
            //make sure we don't execute anything if there was no previous state
            GetCurrentState().ActiveAction = null;

            //remove top state
            States.Pop();

            //enter the new state
            GetCurrentState().OnEnter();
        }

    }
    
    
    
    // Method for switching to new state
    
    public void PushState(Action active, Action onEnter, Action onExit) // push allows to slide new state on top of stack
    {
        // if we have an active state, exit it...
        if (GetCurrentState() != null)
            GetCurrentState().OnExit();

        // ... and construct a new state with the active, Enter and Exit methods
        State state = new State(active, onEnter, onExit);
        
        // ... push new state on top of stack
        States.Push(state);
        
        // ... and call new state's OnEnter method
        GetCurrentState().OnEnter();
    }
    
    
    private State GetCurrentState()
    {
        // check the stack for the currently active state: if there is a state,then add it to the stack
        // the Peek method will give out the first state in the stack, if there is none, it will return null
        
        return States.Count > 0 ? States.Peek() : null; // null is a valid value in a stack --> we therefore need to also check the count to distinguish between a valid value and bottom of the stack


    }
}
