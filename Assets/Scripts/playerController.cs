using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 6f;
    public float rotationSpeed = 7f;
    public float accelerationTime = 0.8f;
    public float decelerationTime = 0.2f;
    private float lastDirection = 0f;
    public float currentSpeed = 0f;
    private bool isRotating = false;
    public float maxRunSpeed = 7f;

    public bool grounded = false;
    private bool isJumping;
    Collider[] groundCollisions;
    private float groundedTimer = 0f;
    public float groundCheckRadius = 0.8f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpSpeed;

    public Transform wallCheck;
    public LayerMask wallLayer;
    public float wallCheckRadius = 0.7f;
    private bool isWall;
    private bool isWallSliding;
    private bool canWallJump;
    public float wallSlidingSpeed = 3f;
    public float wallJumpTime = 0.2f;
    public float wallJumpForce = 8f;
    public float wallJumpHeight = 12f;
    public float wallStickTime = 0.25f;
    public float wallJumpDirection = 1f;
    
    private float wallJumpTimer = 0f;
    private float wallStickTimer = 0f;
    private bool wallJumping = false;

    public Vector2 move;
    public Vector2 look;
    public bool jump;

    Rigidbody rb;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("currentSpeed", Mathf.Abs(currentSpeed));

        move.x = Mathf.Clamp(move.x, -2, 2);
        
        float moveInfluence = wallJumping ? 0.3f : 1f;
        float targetSpeed = move.x * runSpeed * moveInfluence;

        if (Mathf.Abs(move.x) > 0.01f && !isWallSliding)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / accelerationTime);
        }
        else if (!wallJumping)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime / decelerationTime);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxRunSpeed, maxRunSpeed);

        if (wallJumping && wallJumpTimer > 0) 
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0)
            {
                wallJumping = false;
            }
        }
        else if (move.x != 0 && !isWallSliding)
        {
            lastDirection = move.x;
            isRotating = true;
        }

        if (isRotating)
        {
            float targetRotation = Mathf.Sign(lastDirection) * 90f;
            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation, Time.deltaTime * rotationSpeed);
            transform.eulerAngles = new Vector3(0, angle, 0);

            if (Mathf.Abs(transform.eulerAngles.y - targetRotation) < 1f)
            {
                isRotating = false;
            }
        }

        if (isWall)
        {
            wallJumpDirection = lastDirection;
        }

        bool movingTowardsWall = (wallJumpDirection > 0 && move.x > 0) || (wallJumpDirection < 0 && move.x < 0);
        isWallSliding = isWall && !grounded && rb.velocity.y < 0 && movingTowardsWall;

        if (isWallSliding)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue), 0);
            canWallJump = true;
        }
        else
        {
            wallStickTimer = wallStickTime;
            if (isWall && !grounded)
            {
                canWallJump = true;
            }
        }

        if (grounded)
        {
            groundedTimer += Time.deltaTime;
            anim.SetBool("grounded", true);
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
            isJumping = false;
            canWallJump = false;
            wallJumping = false;

            if (jump && groundedTimer >= 0.2f)
            {
                isJumping = true;
                anim.SetBool("isJumping", true);
                rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, 0);
                grounded = false;
                groundedTimer = 0f;
            }
        }
        else
        {
            groundedTimer = 0f;
            anim.SetBool("grounded", false);

            if (jump && canWallJump && isWallSliding)
            {
                float jumpDirectionX = -wallJumpDirection * wallJumpForce;
                rb.velocity = new Vector3(jumpDirectionX, wallJumpHeight, 0);
                
                wallJumping = true;
                wallJumpTimer = wallJumpTime;
                isWallSliding = false;
                canWallJump = false;
                
                lastDirection = -wallJumpDirection;
                isRotating = true;
                
                anim.SetBool("isJumping", true);
            }

            if ((isJumping && rb.velocity.y < 0) || rb.velocity.y < -2)
            {
                anim.SetBool("isFalling", true);
            }
        }
    }

    void FixedUpdate()
    {
        groundCollisions = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        grounded = groundCollisions.Length > 0;

        isWall = Physics.CheckSphere(wallCheck.position, wallCheckRadius, wallLayer);

        if (!wallJumping || wallJumpTimer <= wallJumpTime * 0.5f)
        {
            rb.velocity = new Vector3(currentSpeed, rb.velocity.y, 0);
        }
    }
}