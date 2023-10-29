using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TopDownController : MonoBehaviour
{
    [Range(0,100)]
    public int segments = 10;
    LineRenderer line;
    
    public Slider stunSlider;
    public float StunCooldown = 1.0f;
    public bool canStun = false;
    
    public float stunRadius = 1.0f;
    public bool hasFloppyDisk = false;
    public Transform respawnPosition;
    public AudioSource jumpScareAudio;

    public ParticleSystem stunParticles;
    
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
        //turn stun particles off
        stunParticles.Stop();
        line = GetComponent<LineRenderer>();
        line.SetVertexCount (segments + 1);
        line.useWorldSpace = false;
        
        // =========== MOVEMENT ==============
        rigidbody2d = GetComponent<Rigidbody2D>();
        
        // ==== ANIMATION =====
        animator = GetComponent<Animator>();
    }

    private IEnumerator CreatePoints()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * stunRadius;
            y = Mathf.Cos (Mathf.Deg2Rad * angle) * stunRadius;

            line.SetPosition (i,new Vector3(x,y,0) );

            angle += (360f / segments);
        }

        yield return new WaitForSeconds(0.5f);
        line.enabled = false;
        stunParticles.Stop();
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
        
        //get mouse click
        if (Input.GetMouseButtonDown(0))
        {
            if (!canStun) return;
            canStun = false;
            Debug.Log("Stunning");
            Stun();
            
        }

        // ============== ANIMATION =======================

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", movement.magnitude);
    }

    private void Stun()
    {
        stunParticles.Play();
        stunSlider.value = 0;
        StartCoroutine(StunTimer());
        line.enabled = true;
        StartCoroutine(CreatePoints());
        //cast a circle and check if it hits an enemy
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, stunRadius);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                //get enemy controller
                AIController enemyController = enemy.GetComponent<AIController>();
                //cast to EnemyAIController
                EnemyAIController enemyAIController = (EnemyAIController) enemyController;
                enemyAIController.IsStunned();
            }
        }
        
    }

    private IEnumerator StunTimer()
    {
        yield return new WaitForSeconds(StunCooldown);
        canStun = true;
        
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
