using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public AIState currentState;

    // Update is called once per frame
    protected void Update()
    {
        currentState.UpdateState(this);
    }

    protected void SwitchState(AIState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

}
