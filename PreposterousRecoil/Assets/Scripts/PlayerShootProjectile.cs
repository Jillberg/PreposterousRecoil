using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectile : MonoBehaviour
{
    [SerializeField] private Transform BulletPrefab;
    [SerializeField] private Transform ShootingEffectPrefab;
    [SerializeField] private GameObject RotatePoint;
   
    private void Awake()
    {
        RotatePoint.GetComponent<Aiming>().OnShoot+= PlayerShootProjectile_OnShoot;
    }



    private void PlayerShootProjectile_OnShoot(object sender,Aiming.OnShootEventArgs e)
    {
        Transform bulletTransform=Instantiate(BulletPrefab, e.gunEndPointPosition, Quaternion.identity);
        Vector3 shootDirection = (e.shootPosition - e.gunEndPointPosition).normalized;
        bulletTransform.GetComponent<Bullet>().Setup(shootDirection);
        Transform shootingEffectTransform = Instantiate(ShootingEffectPrefab, e.gunEndPointPosition, Quaternion.identity);
        shootingEffectTransform.GetComponent<ShootingEffect>().Setup(shootDirection);

    }
}
