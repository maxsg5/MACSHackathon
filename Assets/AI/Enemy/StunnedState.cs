using System.Collections;
using Pathfinding;
using UnityEngine;

public class StunnedState : AIState
{
    public float StunDuration = 3.0f;
    // public float closeEnoughDistance = 0.5f;
    public float speed = 0.0f;
    //public float playerDetectionDistance = 5.0f;
    //public MazeGenerator mazeGenerator;  // Reference to your MazeGenerator script
    private Animator animator;

    private AIDestinationSetter _aiDestinationSetter;
    private Transform _playerTransform;

    public override void EnterState(AIController controller)
    {
        //stop the AI from moving
        controller.GetComponent<AIPath>().maxSpeed = speed;
        animator = controller.GetComponent<Animator>();
        animator.SetBool("Stunned", true);
        StartCoroutine(StunTimer(controller));

    }

    private IEnumerator StunTimer(AIController controller)
    {
        yield return new WaitForSeconds(StunDuration);
        animator.SetBool("Stunned", false);
        //switch back to patrol state
        //cast controller to AIEnemyController
        EnemyAIController enemyAIController = (EnemyAIController) controller;
        controller.GetComponent<AIPath>().maxSpeed = speed;
        enemyAIController.GoToWander();
    }

    public override void UpdateState(AIController controller)
    {
        
    }

    public override void ExitState(AIController controller)
    {
        // Exiting logic
    }

    public override Vector2 GetDirection()
    {
        return new Vector2(0, 0);
    }

    public override float GetSpeed()
    {
        return speed;
    }
}