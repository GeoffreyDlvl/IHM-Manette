﻿using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController2D : MonoBehaviour
{
    #region SerializeFields
    [SerializeField, Tooltip("Maximum walking speed in u/s.")]
    float walkingSpeed = 10f;

    [SerializeField, Tooltip("Maximum walking speed in u/s.")]
    float sprintingSpeed = 15f;

    [SerializeField, Tooltip("Dash duration in seconds.")]
    public float dashDuration = .25f;
    public float GetDashDuration() { return dashDuration; }

    [SerializeField, Tooltip("Dash speed in u/s.")]
    float dashSpeed = 30f;

    [SerializeField, Tooltip("Make jumping mid-air possible.")]
    bool allowDashJumping = true;

    [SerializeField, Tooltip("Grounded acceleration when the player moves.")]
    float groundAcceleration = 50f;

    [SerializeField, Tooltip("Grounded deceleration when the player does not input movement.")]
    float groundDeceleration = 80f;

    [SerializeField, Tooltip("Maximum height the player will jump regardless of gravity and on long button press.")]
    float jumpHeight = 5f;

    [SerializeField, Tooltip("Gravity applied to the player. Negative = downward gravity. Positive = upward gravity.")]
    float gravity = -9.81f;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 30f;

    [SerializeField, Range(0f, 1f), Tooltip("Downward drag force applied on the player when pushing against a wall.")]
    float wallDrag = 0.2f;
    
    [SerializeField, Min(0f), Tooltip("Apply multiplier to the player on wall jump. Set to 1.0 to disable this feature.")]
    float wallJumpBoost = 2f;
    
    [SerializeField, Min(0f), Tooltip("Maximum number of consecutive walljumps allowed. Set to zero to disable this feature.")]
    int maxWallJumps = 2;

    [SerializeField]
    Color playerColor;
    [SerializeField]
    Color noMoreWallJumpPlayerColor;

    #endregion

    #region Feedbacks 
    private FeedbackManager feedbackManager;
#endregion

    private Vector2 velocity;
    public Vector2 Velocity
    {
        get
        {
            return this.velocity;
        }
        set
        {
            this.velocity = value;
        }
    }
    IEnumerator dashCoroutine = null;
    int wallJumpCount = 0;
    public bool IsGrounded
    {
        get; private set;
    }

    private bool IsDashing
    {
        get
        {
            return dashCoroutine != null;
        }
        set
        {
            if (true == value)
            {
                this.dashCoroutine = DashCoroutine();
                StartCoroutine(dashCoroutine);
            }
            else if (false == value)
            {
                StopCoroutine(this.dashCoroutine);
                this.dashCoroutine = null;
            }
        }
    }

    private bool canDash;
    private bool IsSprinting
    {
        get { return inputManager.Sprint(); }
    }

    public enum Orientation
    {
        Left = -1
        , Right = 1
    }

    public Orientation orientation
    {
        get; set;
    }

    // Cached variables
    BoxCollider2D boxCollider;
    InputManager inputManager;

    private void ComputeVelocity()
    {
        ComputeXVelocity();
        ComputeYVelocity();
    }

    /// <summary>
    /// Calculates the x velocity to be applied.
    /// 
    /// Different acceleration and deceleration is applied depending on
    /// whether the player is in the air or not, giving a better jump feeling.
    /// </summary>
    private void ComputeXVelocity()
    {
        float xAxis = inputManager.HorizontalAxis();
        UpdatePlayerOrientation(xAxis);

        float acceleration = this.IsGrounded ? this.groundAcceleration : this.airAcceleration;
        float deceleration = this.IsGrounded ? this.groundDeceleration : 0;

        float speed = IsSprinting ? this.sprintingSpeed : this.walkingSpeed;

        bool groundCheck = allowDashJumping ? true : this.IsGrounded;
        if (groundCheck && !this.IsDashing && inputManager.Dash() && canDash)
        {
            this.IsDashing = true;
            this.feedbackManager.PlayFeedback(FeedbackManager.CharacterAction.DASH);
        }

        if (this.IsDashing)
        {           
            float orientationValue = (float)this.orientation;
            this.velocity.x = Mathf.Lerp(this.velocity.x, dashSpeed * orientationValue, acceleration * Time.deltaTime);
        }
        else if (xAxis != 0f)
        {
            this.velocity.x = Mathf.MoveTowards(this.velocity.x, speed * xAxis, acceleration * Time.deltaTime);
        }
        else
        {
            this.velocity.x = Mathf.MoveTowards(this.velocity.x, 0, deceleration * Time.deltaTime);
        }
    }

    private void UpdatePlayerOrientation(float xAxis)
    {
        Orientation previous = orientation;
        this.orientation = xAxis == 0f ? this.orientation : (Orientation)(xAxis / Math.Abs(xAxis));
        transform.localScale = new Vector3((float) this.orientation, transform.localScale.y, transform.localScale.z);
        if (previous != this.orientation && IsGrounded)
        {
            feedbackManager.PlayFeedback(FeedbackManager.CharacterAction.FLIP);
        }
    }

    /// <summary>
    /// Calculates the y velocity to be applied.
    /// 
    /// Y velocity is based on the following formula:
    /// a = (vf² - vo²)/2d
    /// Where : 
    ///     .a is the acceleration, ie the force of gravity;
    ///     .vo is the initial velocity, ie the value we want to solve;
    ///     .vf is the final velocity, ie zero, as there is no motion at the peak of the jump;
    ///     .d is the distance travelled, ie the jump heigh we want to reach.
    /// </summary>
    private void ComputeYVelocity()
    {
        if (this.IsGrounded)
        {
            this.velocity.y = 0;

            if (this.inputManager.JumpPressed())
            {
                this.velocity.y = Mathf.Sqrt(2 * this.jumpHeight * Mathf.Abs(this.gravity));
                this.feedbackManager.PlayFeedback(FeedbackManager.CharacterAction.JUMP);
            }
        }
        
        if (this.inputManager.JumpReleased() && Vector2.Dot(this.velocity, Vector2.up) > 0)
        {
            float jumpValue = this.inputManager.Jump();
            // Min threshold to avoid making tiny jumps on button quick release
            if (jumpValue < .5f) jumpValue = .5f;
            this.velocity.y *= jumpValue;
        }
        
        if (this.IsDashing) return;
        
        this.velocity.y += this.gravity * Time.deltaTime;
    }

    /// <summary>
    /// Detect all collisions with player box collider.
    /// </summary>
    /// <param name="hits">The colliders overlapping the box.</param>
    private void DetectCollisions(out Collider2D[] hits)
    {
        hits = Physics2D.OverlapBoxAll(transform.position, this.boxCollider.size, 0);
    }

    /// <summary>
    /// Uses velocity to translate player.
    /// </summary>
    private void Move()
    {
        transform.Translate(this.velocity * Time.deltaTime);
    }

    /// <summary>
    /// Resolves the collisions by pushing the player out of each collider.
    /// </summary>
    /// <param name="hits">The colliders in contact with the player.</param>
    private void ResolveCollisions(Collider2D[] hits)
    {
        this.IsGrounded = false;
        foreach (Collider2D hit in hits)
        {
            if (ShouldResolveCollisionWith(hit))
            {
                ResolveCollisionWith(hit);
            }
        }
    }

    private bool ShouldResolveCollisionWith(Collider2D hit)
    {
        return hit != this.boxCollider && !hit.isTrigger;
    }

    private void ResolveCollisionWith(Collider2D hit)
    {
        ColliderDistance2D colliderDistance = hit.Distance(this.boxCollider);
        if (colliderDistance.isOverlapped)
        {
            transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

            if (IsCollidingWithGround(colliderDistance))
            {
                this.wallJumpCount = 0;
                this.IsGrounded = true;
                this.canDash = true;
            }
            else if (IsCollidingWithCeiling(colliderDistance))
            {
                this.velocity.y = 0;

            }
            else if (IsCollidingWithWall(colliderDistance))
            {
                if (velocity.y < -2f)
                {
                    ApplyWallDrag();
                }
                AdjustXVelocity();
                if (IsDashing)
                {
                    this.IsDashing = false;
                }
                else if (inputManager.JumpPressed() 
                    && this.wallJumpCount < this.maxWallJumps)
                {
                    if (TooCloseToTheGround())
                        return;

                    this.wallJumpCount++;
                    ComputeWallJumpVelocity();
                }
            }
        }
    }

    private void AdjustXVelocity()
    {
        if (Mathf.Abs(this.velocity.x) > this.walkingSpeed)
        {
            this.velocity.x = this.walkingSpeed * (int)orientation;
        }
    }

    private bool TooCloseToTheGround()
    {
        const float boxCastEpsilon = 0.1f; // Min acceptable distance to the ground
        Vector2 playerCenteredBottomPosition = new Vector2(transform.position.x, transform.position.y - this.boxCollider.size.y / 2);
        Vector2 boxCastSize = new Vector2(this.boxCollider.size.x * .9f, this.boxCollider.size.y); // Slightly narrower to avoid colliding with walls

        RaycastHit2D hit = Physics2D.BoxCast(playerCenteredBottomPosition, boxCastSize, 0f, Vector2.down);

        Debug.Assert(hit.collider != null);
        return hit.distance < boxCastEpsilon;
    }

    private void ApplyWallDrag()
    {
        this.velocity.y *= 1f - this.wallDrag;
        feedbackManager.PlayFeedback(FeedbackManager.CharacterAction.WALLDRAG);
    }

    private void ComputeWallJumpVelocity()
    {
        this.velocity.x = -1 * this.velocity.x * this.wallJumpBoost;
        this.velocity.y = Mathf.Sqrt(2 * this.jumpHeight * Mathf.Abs(this.gravity));
    }

    /// <summary>
    /// Ground is defined as any surface < 90° with the world up.
    /// </summary>
    /// <param name="colliderDistance">The colliding overlap information.</param>
    /// <returns>True if the player is colliding with the ground, false otherwise.</returns>
    private bool IsCollidingWithGround(ColliderDistance2D colliderDistance)
    {
        return Vector2.Angle(colliderDistance.normal, Vector2.up) < 90f;
    }

    /// <summary>
    /// Ceiling is defined as any surface > 90° with the world up.
    /// </summary>
    /// <param name="colliderDistance">The colliding overlap information.</param>
    /// <returns>True if the player is colliding with the ceiling, false otherwise.</returns>
    private bool IsCollidingWithCeiling(ColliderDistance2D colliderDistance)
    {
        return Vector2.Angle(colliderDistance.normal, Vector2.up) > 90f;
    }

    /// <summary>
    /// Walls are defined as any surface of exactly 90° with the world up
    /// </summary>
    /// <param name="colliderDistance"></param>
    /// <returns></returns>
    private bool IsCollidingWithWall(ColliderDistance2D colliderDistance)
    {
        return Vector2.Angle(colliderDistance.normal, Vector2.up) == 90f;
    }

    private IEnumerator DashCoroutine()
    {
        this.velocity.y = 0;
        yield return new WaitForSeconds(this.dashDuration);
        this.IsDashing = false;
        this.canDash = this.IsGrounded;
    }

    private void UpdateRenderer()
    {
        if (this.wallJumpCount >= this.maxWallJumps)
        {
            GetComponent<SpriteRenderer>().color = this.noMoreWallJumpPlayerColor;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = this.playerColor;
        }
    }

    public void ResetPlayer()
    {
        velocity = new Vector2();
        if (IsDashing)
            IsDashing = false;
    }

    #region Unity
    void Awake()
    {
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.inputManager = FindObjectOfType<InputManager>();
        this.feedbackManager = FindObjectOfType<FeedbackManager>();
        this.orientation = Orientation.Right;
    }

    void Update()
    {
        ComputeVelocity();
        Move();
        UpdateRenderer();

        Collider2D[] hits;
        DetectCollisions(out hits);
        ResolveCollisions(hits);
    }
    #endregion
}
