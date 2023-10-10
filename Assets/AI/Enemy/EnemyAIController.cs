using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyAIController : AIController
{
    private enum AIState
    {
        Wander,
        Chase
    };
    //private properties
    [SerializeField]
    private float playerDetectionDistance = 5.0f;
    //states
    private WanderState wanderState;
    private ChaseState chaseState;
    private AIState aiState;
    private AIState previousState;
    // Start is called before the first frame update
    private void Start()
    {      
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //wait a bit then start the initialization
        StartCoroutine(Initialize());
        wanderState = GetComponent<WanderState>(); 
        chaseState = GetComponent<ChaseState>();
        aiState = AIState.Wander;
        SwitchState(wanderState);
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

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
        //CHECK FOR STATE CHANGES
        //if player is in range, switch to chase state
        if(Vector3.Distance(transform.position, playerTransform.position) < playerDetectionDistance)
        {
            aiState = AIState.Chase;
        }
        else
        {
            aiState = AIState.Wander;
        }
        
        
        // Switch state only if state has changed
        if (aiState != previousState)
        {
            switch (aiState)
            {
                case AIState.Wander:
                    SwitchState(wanderState);
                    break;
                case AIState.Chase:
                    SwitchState(chaseState);
                    break;
            }

            previousState = aiState;  // Update previousState to current state
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered");
        if(other.CompareTag("Player"))
        {
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger Exited");
        if(other.CompareTag("Player"))
        {
            
        }
    }
}
