using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public AstarPath Path;
    public AIState currentState;
    public Transform playerTransform;

    // Update is called once per frame
    protected void Update()
    {
        if (currentState == null)
        {
            return;
        }
        currentState.UpdateState(this);
    }

    public void SwitchState(AIState newState)
    {
        if(currentState == null)
        {
            currentState = newState;
            currentState.EnterState(this);
            return;
        }
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

}
