using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : StateManager.cs
==============================
*/
public class StateManager : MonoBehaviour
{
    [SerializeField] 
    private State currentState = null;

    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();

        if(nextState != null)
        {
            SwitchToTheNextState(nextState);
        }
    }

    private void SwitchToTheNextState(State nextState)
    {
        currentState = nextState;
    }
    public State GetCurrentState()
    {
        return currentState;
    }
    public void SetCurrentState(State _state)
    {
        currentState = _state;
    }
}
