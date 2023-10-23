using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAIController : AIController
{
    public enum AIState
    {
        Patrol,
        Talk,
        FinalTalk
    };
    
    //private properties
    [SerializeField]
    private float playerDetectionDistance = 5.0f;
    private bool isTalking = false;
    //states
    private PatrolState patrolState;
    private TalkState talkState;
    private FinalTalkState finalTalkState;
    public AIState aiState;
    private AIState previousState;
    private bool finalTalk = false;
    

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //wait a bit then start the initialization
        StartCoroutine(Initialize());
        patrolState = GetComponent<PatrolState>();
        talkState = GetComponent<TalkState>();
        finalTalkState = GetComponent<FinalTalkState>();
        SwitchState(patrolState);
    }

    private new void Update()
    {
        base.Update();
        
        //get distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        //check if player has floppy disk
        if (playerTransform.GetComponent<TopDownController>().hasFloppyDisk && finalTalk == false && distanceToPlayer <= playerDetectionDistance )
        {
            finalTalk = true;
            //if player has floppy disk, switch to talk state
            aiState = AIState.FinalTalk;
        }
        
        
        //switch state only if the state has changed
        if (aiState != previousState)
        {
            switch (aiState)
            {
                case AIState.Patrol:
                    SwitchState(patrolState);
                    break;
                case AIState.Talk:
                    SwitchState(talkState);
                    break;
                case AIState.FinalTalk:
                    SwitchState(finalTalkState);
                    break;
            }
        }
        previousState = aiState; //update previous state to current state
        
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
    
    //check for box collision with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTalking)
        {
            aiState = AIState.Talk;
            isTalking = true;
        }
    }
}
