using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class AIDirectionalAnimation : MonoBehaviour
{
    
    private Animator animator;
    private AIController _aiController;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        _aiController = GetComponent<AIController>();
    }

    void Update()
    {
        //check if we have a current state
        if (_aiController.currentState == null)
        {
            return;
        }
        Vector2 direction = _aiController.currentState.GetDirection(); // We will define this method
        float speed = _aiController.currentState.GetSpeed(); // We will define this method
        float horizontal = direction.x;
        float vertical = direction.y;
        
        //if horizontal is greater than vertical, we are moving horizontally
        if(Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            vertical = 0;
        }
        else
        {
            horizontal = 0;
        }

        animator.SetFloat("Look X", horizontal);
        animator.SetFloat("Look Y", vertical);
        animator.SetFloat("Speed", speed);
    }
}
