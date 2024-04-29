using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float bounceForce;
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            HandlePlayerBeingHit(collision.gameObject);
        }
    }

    private void HandlePlayerBeingHit(GameObject player)
    {
        if (player != null) 
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                rb.velocity=new Vector2(rb.velocity.x,0f);
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}
