using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform movingPointA;
    public Transform movingPonitB;
    public float movingSpeed;

    private Vector3 nextPoint;

    void Start()
    {
        nextPoint=movingPointA.position;
    }

    void Update()
    {
        transform.position=Vector3.MoveTowards(transform.position,nextPoint,movingSpeed*Time.deltaTime);

        if (transform.position == nextPoint)
        {
           nextPoint= (nextPoint==movingPointA.position)? movingPonitB.position: movingPointA.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent=transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(gameObject.activeInHierarchy) {

            if (collision.gameObject.CompareTag("Player"))
            {
                StartCoroutine(DetachPlayer(collision.gameObject));
            }
        }
        
    }

    IEnumerator DetachPlayer(GameObject player)
    {
        yield return new WaitForEndOfFrame(); // Wait for the end of the frame
        if (player != null)
        {
            player.transform.SetParent(null, true); // Detach the player
        }
    }
}
