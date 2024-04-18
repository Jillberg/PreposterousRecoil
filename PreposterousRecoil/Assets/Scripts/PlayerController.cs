using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    Transform rotatePoint;
    private bool canMove = true;


    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;
    private bool isFacingRight = true;

    

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
            Flip();
        }


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
            cls.x *= -1;
            cls.y *= -1;
            transform.localScale = ls;
            rotatePoint.localScale = cls;

        }
    }
}
