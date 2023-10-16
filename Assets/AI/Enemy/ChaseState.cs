using System.Collections;
using Pathfinding;
using UnityEngine;

public class ChaseState : AIState
{
    public float closeEnoughDistance = 0.5f;
    public float speed = 5.0f;
    //public float playerDetectionDistance = 5.0f;
    //public MazeGenerator mazeGenerator;  // Reference to your MazeGenerator script

    private AIDestinationSetter _aiDestinationSetter;
    private Transform _playerTransform;

    public override void EnterState(AIController controller)
    {
        controller.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _aiDestinationSetter = controller.GetComponent<AIDestinationSetter>();
        _playerTransform = controller.playerTransform;
        //set the target to the player
        _aiDestinationSetter.target = _playerTransform;
        //increase the speed of the AI
        controller.GetComponent<AIPath>().maxSpeed = speed;
    }

    public override void UpdateState(AIController controller)
    {
        if (_playerTransform == null)
        {
            return;
        }
        //float distanceToTarget = Vector3.Distance(controller.transform.position, _aiDestinationSetter.target.position);
        float distanceToPlayer = Vector3.Distance(controller.transform.position, _playerTransform.position);

        if (distanceToPlayer < closeEnoughDistance)
        {
            //TODO: either attack player, play jumpscare, or something else
        }
    }

    public override void ExitState(AIController controller)
    {
        // Exiting logic
    }

    public override Vector2 GetDirection()
    {
        return (_aiDestinationSetter.target.position - transform.position).normalized;
    }
}