using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAIController : AIController
{
    private enum AIState
    {
        Patrol,
        Talk
    };
    
    //private properties
    [SerializeField]
    private float playerDetectionDistance = 5.0f;
    
    //states
    private PatrolState patrolState;
    private AIState aiState;
    private AIState previousState;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //wait a bit then start the initialization
        StartCoroutine(Initialize());
        patrolState = GetComponent<PatrolState>();
        SwitchState(patrolState);
    }
    
    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(1.0f);
    }
    
    private void OnDrawGizmos()
    {
        //draw debug circle for player detection
        Gizmos.DrawWireSphere(transform.position, playerDetectionDistance);
    }
}
