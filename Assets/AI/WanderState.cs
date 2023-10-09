using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : AIState
{
    public Transform[] waypoints;
    public float speed = 3.0f;
    public float reachDistance = 0.5f;
    
    private int currentWaypoint = 0;

    public override void EnterState(AIController controller)
    {
        // Logic stays the same, no need to set destination since we're manually handling movement
    }

    public override void UpdateState(AIController controller)
    {
        Vector3 direction = (waypoints[currentWaypoint].position - controller.transform.position).normalized;
        float distanceToWaypoint = Vector3.Distance(controller.transform.position, waypoints[currentWaypoint].position);

        // Move towards the waypoint
        controller.transform.position += direction * (speed * Time.deltaTime);

        // Check if AI is close to the current waypoint
        if(distanceToWaypoint <= reachDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    public override void ExitState(AIController controller)
    {
        // Exiting logic
    }

    public override Vector3 GetDirection()
    {
        return (waypoints[currentWaypoint].position - transform.position).normalized;
    }
}
