using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    Transform rotatePoint;
    private bool canMove = true;

    public static event Action OnGroundCheck;
    public static event Action IsAirborne;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;
    private bool isFacingRight = true;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize=new Vector2(0.5f,0.5f);
    public LayerMask groundLayer;
    private bool wasGroundedLastFrame = false;

    [Header("Gravity")]
    public float baseGravity = 10f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;



    void OnEnable()
    {
        Aiming.OnRecoilStart += HandleRecoilStart;
        Aiming.OnRecoilEnd += HandleRecoilEnd;
    }

    void OnDisable()
    {
        Aiming.OnRecoilStart -= HandleRecoilStart;
        Aiming.OnRecoilEnd -= HandleRecoilEnd;
    }

    private void HandleRecoilStart()
    {
        canMove = false;
    }

    private void HandleRecoilEnd()
    {
        canMove = true;
    }


    void Start()
    {
        rotatePoint = transform.Find("RotatePoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (horizontalMovement != 0&&canMove)
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
           // Flip();
        }

    }

    void FixedUpdate()
    {
        SendOnGroundCheckEvent();
        SendIsAirborneEvent();
        ProcessGravity();
    }
    


    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    private void Flip()
    {
        if (!isFacingRight && horizontalMovement > 0 || isFacingRight && horizontalMovement < 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            Vector3 cls = rotatePoint.localScale;
            ls.x *= -1;
           // cls.x *= -1;
           // cls.y *= -1;
            transform.localScale = ls;
         //   rotatePoint.localScale = cls;
         
        }
    
    }

    private bool IsGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            
            return true;
        }
        return false;
    }

    private void SendOnGroundCheckEvent()
    {
        bool currentStatus = IsGrounded();
        if (currentStatus&&! wasGroundedLastFrame)
        {
            OnGroundCheck?.Invoke();
            Debug.Log("Just touched the ground");
            Debug.Log(rb.velocity.y);
        }
        wasGroundedLastFrame= currentStatus;
    }

    private void SendIsAirborneEvent()
    {
        if (!IsGrounded())
        {
            IsAirborne?.Invoke();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);

    }

    private void ProcessGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;//make falling faster
            //rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }
}
