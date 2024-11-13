using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    public float speed = 250.0f;
    public float jumpHeight = 2.5f;
    public float life = 100.0f;
    private bool isJumping = false;
    private float horizontalInput = 0.0f;
    private bool jumpInput = false;
    private bool attackInput = false;
    private float jumpImpulse;
    public float score = 0.0f;
    public GameObject ScoreCounterObject;
    public GameObject LifeCounterObject;
    private TextMeshProUGUI scoreCounter;
    private TextMeshProUGUI lifeCounter;
    private AudioSource m_AudioSource;
    public AudioClip impact;

    // Start is called before the first frame update
    void Start()
    {
        // Initializing Components
        rb2D = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();

        //Lock the rigidbody2D rotation to avoid bugs when touching the edges of platforms.
        rb2D.freezeRotation = true;

        // Setting the jump impulse according to the kinematic equations. The actual height is a bit lower, depending on the gravity applied to the character.
        jumpImpulse = rb2D.mass * Mathf.Sqrt(2.0f * Mathf.Abs(Physics2D.gravity.y) * rb2D.gravityScale * jumpHeight);

        // Initialize the UI elements.
        scoreCounter = ScoreCounterObject.GetComponent<TextMeshProUGUI>();
        scoreCounter.text = "Score: " + score;
        lifeCounter = LifeCounterObject.GetComponent<TextMeshProUGUI>();
        lifeCounter.text = "Life: " + life;

    }

    // Update is called once per frame
    void Update()
    {
        // Inputs
        horizontalInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        if (Input.GetKey("x"))
        {
            attackInput = true;
        }
        else
        {
           attackInput = false;
        }



        // Regular Jump Behavior
        if (jumpInput && isJumping == false)
        {
            Jump();
            animator.SetBool("isJumping", true);
        }

    }

    private void FixedUpdate()
    {

        // Horizontal Movement
        rb2D.velocity = new Vector2(horizontalInput * speed * Time.fixedDeltaTime, rb2D.velocity.y);

        //Animation
        if (horizontalInput < 0)
        {
            animator.SetBool("isWalking", true);
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput > 0)
        {
            animator.SetBool("isWalking", true);
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput == 0)
        {
            animator.SetBool("isWalking", false);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
            
        if (collision.gameObject.tag == "Floor")
        {          
            // Recharge Jumping upon touching a floor.
            isJumping = false;
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);

            // Exit jump animations
            animator.SetBool("isJumping", false);
        }

        // Remove life upon collision with a hazard
        if (collision.gameObject.CompareTag("Hazard"))
        {
            life -= 10f;
            lifeCounter.text = "Life: " + life;
        }

        // Collect power ups and increase the score
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            score += 10.0f;
            scoreCounter.text = "Score: " + score;
            Destroy(collision.gameObject);
            
        }

        // End the game upon touching the treasure.
        if (collision.gameObject.CompareTag("Treasure"))
        {
            Debug.Log("End Game");
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }

        // Restore life upon touching the platform.
        if (collision.gameObject.CompareTag("Platform"))
        {
            life = 100f;
            lifeCounter.text = "Life: " + life;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Keep the jumping reset when in collision with a "Floor" object, to avoid a bug where Jump is disabled when exiting a collision while still colliding with a different object.
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {          
            // Enable isJumping so it can't jump again after falling from a platform.
            isJumping = false;
        }
    }

    private void OnTriggerStay2D (Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Enemy"))
        {
            // When the attack trigger is on an enemy and the attack key is pressed, destroy the enemy.
            if(attackInput)
            {
                Destroy(trigger.gameObject);
                m_AudioSource.PlayOneShot(impact);
            }
        }
    }
    
    private void Jump()
    {
        rb2D.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
        isJumping = true;
    }
}