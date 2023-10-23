using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
using Object = UnityEngine.Object;

public class FinalTalkState : AIState
{
    public float speed = 3.0f;
    public float closeEnoughDistance = 0.5f;
    public GameObject dialogueBox;
    private MazeGenerator mazeGenerator;  
    private GameObject targetPoint;
    //private AIDestinationSetter _aiDestinationSetter;
    private Transform _playerTransform;
    
    private AIController _aiController;
    private TopDownController _playerController;    
    private GridGraph _gridGraph;
    
    private bool _isTalking = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, closeEnoughDistance);
    }

    public override void EnterState(AIController controller)
    {
        controller.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _playerTransform = controller.playerTransform;
        _aiController = controller;

        
        //disable player movement
        _playerController = _playerTransform.GetComponent<TopDownController>();
        _playerController.canMove = false;
        
    }

    public override void UpdateState(AIController controller)
    {
        if (_isTalking)
        {
            //check if the dialogue box is closed
            if (!dialogueBox.activeSelf)
            {
                //switch back to patrol state
                _isTalking = false;
                _playerController.canMove = true; 
                //cast controller to NPCAIController
                NPCAIController npcController = (NPCAIController) controller;
                npcController.aiState = NPCAIController.AIState.Patrol;
            }
            return;
        }
        //float distanceToTarget = Vector3.Distance(controller.transform.position, _aiDestinationSetter.target.position);
        float distanceToPlayer = Vector3.Distance(controller.transform.position, _playerTransform.position);

        if (distanceToPlayer < closeEnoughDistance)
        { 
            //open dialogue
            dialogueBox.SetActive(true);
            speed = 0.0f;
            _isTalking = true;
        }
        
        //move towards the player
        Vector3 direction = (_playerTransform.position - controller.transform.position).normalized;
        controller.transform.position += direction * (speed * Time.deltaTime);
    }

    public override void ExitState(AIController controller)
    {
        // Destroy the target point GameObject when exiting the state
        if (targetPoint != null)
            Object.Destroy(targetPoint);
    }

    public override Vector2 GetDirection()
    {
        return (_playerTransform.position - transform.position).normalized;
    }

    public override float GetSpeed()
    {
        return speed;
    }
}