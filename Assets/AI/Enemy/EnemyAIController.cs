using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAIController : AIController
{
    
    //private properties
    [SerializeField]
    private float playerDetectionDistance = 5.0f;
    //states
    private WanderState wanderState;
    private ChaseState chaseState;
    
    // Start is called before the first frame update
    private void Start()
    {      
        //wait a bit then start the initialization
        StartCoroutine(Initialize());
        wanderState = GetComponent<WanderState>(); 
        chaseState = GetComponent<ChaseState>();
        SwitchState(chaseState);
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
        
        //if player is in range, switch to chase state
        // if(Vector3.Distance(transform.position, playerTransform.position) < playerDetectionDistance)
        // {
        //     SwitchState(chaseState);
        // }
        // else
        // {
        //     SwitchState(wanderState);
        // }
        
        
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
