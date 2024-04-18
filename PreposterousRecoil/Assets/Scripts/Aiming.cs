using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public static event Action OnRecoilStart;
    public static event Action OnRecoilEnd;

    public Rigidbody2D rb;  // Reference to the Rigidbody2D component
    public float recoilStrength = 5f;  // Adjustable recoil strength
    //private bool gunIsFacingRight = true;
    private bool isReloading = false;
    public float reloadTime = 1f;
    private int ammo;
    public int maximumAmmo = 2;
    Transform gunPosition;


    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        ammo = maximumAmmo;
        gunPosition = transform.Find("BulletTransform");
    }

    private bool shouldFire = false;
    private bool shouldReload = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ammo > 0 && !isReloading)
        {
            shouldFire = true;
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            shouldReload = true;
        }
    }

    void FixedUpdate()
    {
        if (shouldFire)
        {
            Fire();
            shouldFire = false; // Reset the flag
        }

        if (shouldReload)
        {
            StartCoroutine(Reload());
            shouldReload = false; // Reset the flag
        }

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 rotation = mousePos - transform.position;
        float rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotz);
    }

    private void Fire()
    {
            ammo--;
            ApplyRecoil();
  
        
    }
    IEnumerator Reload() {
        isReloading = true;
        Debug.Log("Reloading...");

        // Wait for reload time
        yield return new WaitForSeconds(reloadTime);

        ammo = maximumAmmo; // Reset ammo count
        isReloading = false;
        Debug.Log("Reloaded");
    }

    void ApplyRecoil()
    {
        Vector2 fireDirection = (mousePos - transform.position).normalized;
        fireDirection *= -1;
        rb.velocity = Vector2.zero;
        rb.AddForce(fireDirection * recoilStrength, ForceMode2D.Impulse);

        OnRecoilStart?.Invoke();

        // Assuming the recoil effect lasts 0.5 seconds, adjust as needed
        StartCoroutine(EndRecoil());
    }

    IEnumerator EndRecoil()
    {
        yield return new WaitForSeconds(0.2f);
        OnRecoilEnd?.Invoke();
    }



    /* private void GunFlip()
      {
          if(mousePos.x < transform.position.x && gunIsFacingRight||mousePos.x>transform.position.x&&!gunIsFacingRight){
              gunIsFacingRight=!gunIsFacingRight;
              Vector3 ls = transform.localScale;

              ls.y *= -1;
              transform.localScale=ls;

          }
      }*/


}

