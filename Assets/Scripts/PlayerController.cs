﻿using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashSpeed = 20f;
    [SerializeField] float dashDistance = 0.01f;
    [SerializeField] float dashIncrement = 0.5f;
    [SerializeField] float sprintSpeed = 30f;
    [SerializeField] float impulse = 10f;
    [SerializeField] float jumpMultiplier = 2f;
    [SerializeField] float fallMultiplier = 3f;
    [SerializeField] float gravity = 9.81f;

    
    float currentDashTime;
    bool isGrounded = true;
    float yVelocity;
    float groundYPosition; // Store ground y position below the player

    // Cached variables
    Rigidbody2D rigidbody;
    BoxCollider2D collider;
    float playerHeight;

    private void Start()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.collider = GetComponent<BoxCollider2D>();
        
        this.playerHeight = collider.size.y;
        this.groundYPosition = transform.position.y - playerHeight / 2;
        this.currentDashTime = 0;
    }

    void Update()
    {
        CheckGroundContact();
        Move();
        Jump();
        Dash();
    }

    private void CheckGroundContact()
    {
        RaycastHit2D hit = Physics2D.Raycast(FeetPosition(), Vector2.down);

        if (hit.collider != null)
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if (hit.distance <= Mathf.Epsilon)
            {
                this.isGrounded = true;
                AdjustYPosition();
            }
            else
            {
                this.isGrounded = false;
                this.groundYPosition = hit.point.y;
            }
        }
    }

    private void AdjustYPosition()
    {
        float feetYPosition = FeetPosition().y;
        if (feetYPosition < this.groundYPosition)
        {
            float groundIntersection = this.groundYPosition - feetYPosition;
            transform.position += new Vector3(0f, groundIntersection);
        }
    }

    private Vector2 FeetPosition()
    {
        return new Vector2(transform.position.x, transform.position.y - this.playerHeight / 2f);
    }

    private void Jump()
    {
        if (this.isGrounded && Input.GetButton("Jump"))
        {
            this.yVelocity = this.impulse;
        }
        else if (isGrounded)
        {
            this.yVelocity = 0;
        }
        else
        {
            float yVelocityModifier = this.gravity * Time.deltaTime;
            if (this.yVelocity > 0)
            {
                yVelocityModifier *= this.jumpMultiplier;
            }
            if (this.yVelocity < 0)
            {
                yVelocityModifier *= this.fallMultiplier;
            }
            this.yVelocity -= yVelocityModifier;
        }

        transform.position += transform.up * this.yVelocity * Time.deltaTime;
    }

    private void Move()
    {
        float xAxis = Input.GetAxis("Joystick X");
        if (Input.GetButton("Sprint"))
        {
            Move(xAxis, sprintSpeed);
        } else
        {
            Move(xAxis, speed);
        }
    }

    private void Move(float xAxis, float desiredSpeed)
    {
        transform.position += new Vector3(xAxis, 0f) * Time.deltaTime * desiredSpeed;
    }

    private void MoveTo(Vector3 location, float speed)
    {
        transform.position += location * speed * Time.deltaTime;
    }

    private void Dash()
    {
        float xAxis = Input.GetAxis("Joystick X");
        if (Input.GetButtonDown("Dash") && xAxis != 0) {
            Dash(xAxis);
        } else {
            currentDashTime = 0f;
        }
    }

    private void Dash(float xAxis)
    {        
        while(currentDashTime < dashDuration )
        {
            float absoluteMovingDirection = (xAxis / Math.Abs(xAxis));
            Vector3 moveDirection;  moveDirection = new Vector3((absoluteMovingDirection * dashDistance) * dashIncrement, 0.0f);
            MoveTo(moveDirection, dashSpeed);
            currentDashTime += dashIncrement;
        }
    }
}
 