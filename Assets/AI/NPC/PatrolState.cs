using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
using Object = UnityEngine.Object;

public class PatrolState : AIState
{
    public Transform[] patrolPoints;
    public Transform target;
    public float speed = 3.0f;
    public float closeEnoughDistance = 0.5f;
    private MazeGenerator mazeGenerator;  
    private GameObject targetPoint;
    private AIDestinationSetter _aiDestinationSetter;
    private Transform _playerTransform;
    private int targetIndex = 0;
    
    private AIController _aiController;
    // private GridGraph _gridGraph;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeEnoughDistance);
    }

    public override void EnterState(AIController controller)
    {
        controller.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        mazeGenerator = GameObject.FindObjectOfType<MazeGenerator>();
        _aiDestinationSetter = controller.GetComponent<AIDestinationSetter>();
        _playerTransform = controller.playerTransform;
        _aiController = controller;
        //targetPoint = new GameObject("TargetPoint");
        //_aiDestinationSetter.target = targetPoint.transform;  // Set the new GameObject as the target
        controller.GetComponent<AIPath>().maxSpeed = speed;
        PickNewDestination();
    }

    public override void UpdateState(AIController controller)
    {
        float distanceToTarget = Vector3.Distance(controller.transform.position, target.position);
        float distanceToPlayer = Vector3.Distance(controller.transform.position, _playerTransform.position);

        if (distanceToTarget < closeEnoughDistance)
        {
            PickNewDestination();
        }
        
        //move towards the target
        Vector3 direction = (target.position - controller.transform.position).normalized;
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
        return (target.position - transform.position).normalized;
    }

    // private void PickNewDestinationInMaze()
    // {
    //     Room randomRoom = mazeGenerator.GetRandomRoom();  // Assuming you add a method to get a random room
    //     Vector3Int roomCenter = randomRoom.GetCenterCoords();
    //     Vector3 newPosition = (Vector3)roomCenter + new Vector3(0.5f, 0.5f, 0);  // Center of a tile within the room
    //     _aiDestinationSetter.target.position = newPosition; 
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

    private void PickNewDestination()
    {
        target = patrolPoints[targetIndex];
        if (targetIndex + 1 > patrolPoints.Length-1)
        {
            targetIndex = 0;
        }
        else
        {
            targetIndex++;
        }
        
    }
}