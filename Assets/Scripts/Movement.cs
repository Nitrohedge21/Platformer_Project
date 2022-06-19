using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    Vector2 move;
    [SerializeField] public int speed;

    [Header("Jump System")]
    [SerializeField] public int jumpHeight;
    [SerializeField] public float jumpTime;
    [SerializeField] public float jumpMultiplier;
    [SerializeField] public float fallMultiplier;
    [SerializeField] private LayerMask jumpableGround;

    Vector2 vecGravity;
    bool isJumping;
    float jumpCounter;

    //private Animator anim;
    float directionX = 0f;
    
    

    private BoxCollider2D coll;
    //private enum MovementState { idle, running, jumping, falling }

    private SpriteRenderer sprite;

    void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        //anim = GetComponent<Animator>();
    }


    void Update()
    {
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // Used GetAxis instead of GetAxisRaw to get a smoother movement.

        transform.Translate(move * speed * Time.deltaTime);

        flip();

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            isJumping = true;
            jumpCounter = 0;
        }

        //UpdateAnimations();
        if (rb.velocity.y > 0 && isJumping)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime)
            {
                isJumping = false;
            }
            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (t > 0.5f)
            {
                currentJumpM = jumpMultiplier * (1 - t);
            }
            rb.velocity += vecGravity * currentJumpM * Time.deltaTime;
        }
        
        if(Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpCounter = 0;

            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
            }
        }

        if (rb.velocity.y < 0)  
        {
            // This could be changed to else if but idk why the guy in the tutorial isn't doing it.
            rb.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
            // This is basically the same thing as changing the gravity scale but this one only affects the player object.
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(move * speed);
        // Check the tutorial to make it more complicated if needed.
        // you can change the way how velocity works by ForceMode2D.Force or ForceMode2D.Impulse - 19.06.2022
        
    }
    


    void flip()
    {
        if (move.x < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        else if (move.x > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    //private void UpdateAnimations()
    //{
    //    MovementState state;

    //    if (directionX > 0f)
    //    {

    //        state = MovementState.running;

    //        sprite.flipX = true;
    //    }
    //    else if (directionX < 0f)
    //    {
    //        state = MovementState.running;

    //        sprite.flipX = false;
    //        //swap the true and false depending on the way sprite is facing

    //    }
    //    else
    //    {
    //        state = MovementState.idle;
    //    }

    //    if (rb.velocity.y > .1f)
    //    {
    //        state = MovementState.jumping;
    //    }

    //    else if (rb.velocity.y < -.1f)
    //    {
    //        state = MovementState.falling;
    //    }

    //    anim.SetInteger("State", (int)state);

    //}

    // Ground check could perhaps be changed/updated later on if it's ever needed? - 19.06.2022
    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

}
