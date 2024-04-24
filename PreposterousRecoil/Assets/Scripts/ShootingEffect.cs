using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShootingEffect : MonoBehaviour
{
    private Vector3 shootDir;

  
    public void Setup(Vector3 shootDirection)
    {
        this.shootDir = shootDirection;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
        Destroy(gameObject, Time.deltaTime*5f);
    }


    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
