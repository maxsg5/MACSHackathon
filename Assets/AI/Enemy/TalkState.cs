using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
using Object = UnityEngine.Object;

public class TalkState : AIState
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
        //mazeGenerator = GameObject.FindObjectOfType<MazeGenerator>();
        //_aiDestinationSetter = controller.GetComponent<AIDestinationSetter>();
        _playerTransform = controller.playerTransform;
        _aiController = controller;
        //targetPoint = new GameObject("TargetPoint");
        //_aiDestinationSetter.target = targetPoint.transform;  // Set the new GameObject as the target
        //controller.GetComponent<AIPath>().maxSpeed = speed;
        //PickNewDestination();
        
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
                _playerController.canStun = true;
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

    
    
    // private void PickNewDestination()
    // {
    //     GridGraph gridGraph = _aiController.Path.data.gridGraph;
    //     GraphNode randomNode;
    //     do
    //     {
    //         int randomX = UnityEngine.Random.Range(0, gridGraph.width);
    //         int randomZ = UnityEngine.Random.Range(0, gridGraph.depth);  // Using 'Z' as it's a 2D grid
    //         randomNode = gridGraph.GetNode(randomX, randomZ);
    //     } while (randomNode == null || !randomNode.Walkable);
    //
    //     Vector3 randomWalkablePosition = (Vector3)randomNode.position;
    //     _aiDestinationSetter.target.position = randomWalkablePosition;
    // }
    
    // private void PickNewDestination()
    // {
    //     GridGraph gridGraph = _aiController.Path.data.gridGraph;
    //     GraphNode randomNode;
    //     do
    //     {
    //         int randomX = UnityEngine.Random.Range(0, gridGraph.width);
    //         int randomZ = UnityEngine.Random.Range(0, gridGraph.depth);  // Using 'Z' as it's a 2D grid
    //         randomNode = gridGraph.GetNode(randomX, randomZ);
    //     } while (randomNode == null || !randomNode.Walkable);
    //
    //     Vector3 randomWalkablePosition = (Vector3)randomNode.position;
    //     targetPoint.transform.position = randomWalkablePosition;  // Update the position of the target point
    // }
}