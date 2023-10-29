using System;
using System.Collections;
using UnityEngine;

public class TopDownController : MonoBehaviour
{
    public bool hasFloppyDisk = false;
    public Transform respawnPosition;
    public AudioSource jumpScareAudio;
    
    // ========= MOVEMENT =================
    public float speed = 4;
    public bool canMove = true;
    
    // =========== MOVEMENT ==============
    Rigidbody2D rigidbody2d;
    private Vector2 movement;

    // ==== ANIMATION =====
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    void Start()
    {
        // =========== MOVEMENT ==============
        rigidbody2d = GetComponent<Rigidbody2D>();
        
        // ==== ANIMATION =====
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //disable movement
        if (!canMove)
        {
            movement = Vector2.zero;
            animator.SetFloat("Speed", 0);
            return;
        }
        
        // ============== MOVEMENT ======================
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
                
        movement = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(movement.x, 0.0f) || !Mathf.Approximately(movement.y, 0.0f))
        {
            lookDirection.Set(movement.x, movement.y);
            lookDirection.Normalize();
        }

        // ============== ANIMATION =======================

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", movement.magnitude);
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        
        position = position + movement * speed * Time.deltaTime;
        
        rigidbody2d.MovePosition(position);
    }

    public void Respawn() 
    {
        jumpScareAudio.Play();
        canMove = false;
        transform.position = respawnPosition.position;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait() 
    {
        yield return new WaitForSeconds(0.5f);
        LevelManager.Instance.HideScare();
        canMove = true;
    }
}
