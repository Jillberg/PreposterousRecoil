using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformCollision : MonoBehaviour
{
    public float existingTime;
    public float respawnTime;
    //[SerializeField] private GameObject platform;
    


   
   
    void Start()
    {
     
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("happened");
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ProcessFalling());
        }
    }

  

    private IEnumerator ProcessFalling()
    {
        yield return new WaitForSeconds(existingTime);

        gameObject.GetComponent<BoxCollider2D>().enabled=false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Debug.Log("destroyed");

        yield return new WaitForSeconds(respawnTime);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Debug.Log("respawn");
    }

   
}
