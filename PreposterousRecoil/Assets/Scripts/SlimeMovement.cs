using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    private GameObject player;
    private Transform playerTransform;

    public float chaseSpeed;
    public float jumpForce;

    private Rigidbody2D rb;
    private bool shouldJump=true;
    private bool isGrounded=false;


    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerTransform = player.GetComponent<Transform>();
            if(playerTransform != null )
            {
                Debug.Log("!!!!!");
            }
        }
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
        
           rb.velocity=new Vector2(chaseSpeed*direction, rb.velocity.y);
        
    }

    void FixedUpdate()
    {
        Debug.Log(isGrounded);
        if (shouldJump&&isGrounded)
        {
            
            Jump();
        }
    }

    private void Jump()
    {
        
        Vector2 direction=(transform.position-playerTransform.position).normalized;
        rb.AddForce(new Vector2((direction * -jumpForce).x,jumpForce),ForceMode2D.Impulse);
        isGrounded = false;
        
            
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Collider2D collider = collision.collider;
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.layer != LayerMask.NameToLayer("Wall"))
        {
            //isGrounded = true;
        }
    }
}
