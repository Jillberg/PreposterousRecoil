using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDir;
    
    public float bulletSpeed = 20f;
 


    public void Setup(Vector3 shootDirection)
    {
       
            this.shootDir = shootDirection;
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(shootDir * bulletSpeed, ForceMode2D.Impulse);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
            Destroy(gameObject, 5f);
        
        
    }

   /*private void Update()
    {
        transform.position += shootDir * Time.deltaTime*10f;
    }*/

   private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
            /* Target target = collision.GetComponent<Target>();
             if (target!= null) {
                 target.Damage();
             }*/
            if (collision.GetComponent<Mesh>() != null)
            {
                Destroy(gameObject);
                Debug.Log(collision);

            
        }
        
        

    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir= dir.normalized;
        float n=Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
