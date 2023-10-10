using System.Collections;
using Pathfinding;
using UnityEngine;

public class WanderState : AIState
{
    public float closeEnoughDistance = 0.5f;
    private MazeGenerator mazeGenerator;  

    private AIDestinationSetter _aiDestinationSetter;
    private Transform _playerTransform;

    public override void EnterState(AIController controller)
    {
        mazeGenerator = GameObject.FindObjectOfType<MazeGenerator>();
        _aiDestinationSetter = controller.GetComponent<AIDestinationSetter>();
        _playerTransform = controller.playerTransform /* get the player's transform somehow */;
        PickNewDestination();
    }

    public override void UpdateState(AIController controller)
    {
        float distanceToTarget = Vector3.Distance(controller.transform.position, _aiDestinationSetter.target.position);
        float distanceToPlayer = Vector3.Distance(controller.transform.position, _playerTransform.position);

        if (distanceToTarget < closeEnoughDistance)
        {
            PickNewDestination();
        }
    }

    public override void ExitState(AIController controller)
    {
        // Exiting logic
    }

    public override Vector3 GetDirection()
    {
        return (_aiDestinationSetter.target.position - transform.position).normalized;
    }

    private void PickNewDestination()
    {
        Room randomRoom = mazeGenerator.GetRandomRoom();  // Assuming you add a method to get a random room
        Vector3Int roomCenter = randomRoom.GetCenterCoords();
        Vector3 newPosition = (Vector3)roomCenter + new Vector3(0.5f, 0.5f, 0);  // Center of a tile within the room
        _aiDestinationSetter.target.position = newPosition; 
    }
}