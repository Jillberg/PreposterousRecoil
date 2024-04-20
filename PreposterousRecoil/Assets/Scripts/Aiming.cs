using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aiming : MonoBehaviour
{
    //should not mess around with these guys
    private Camera mainCam;
    private Vector3 mousePos;
    public static event Action OnRecoilStart;
    public static event Action OnRecoilEnd;
    private Transform gunPosition;
    public Rigidbody2D rb;  // Reference to the Rigidbody2D component

    [Header("RecoilParameters")]
    public float recoilStrength = 5f;  // Adjustable recoil strength
    //private bool gunIsFacingRight = true;

    [Header("Ammunition")]
    private bool isReloading = false;
    public float reloadTime = 1f;
    private int ammo;
    public int maximumAmmo = 2;
    private bool shouldReload = false;
    private bool magazineIsFull = true;
    private bool canReload;

    [Header("LoadingImage")]
    public Image loadingCircle;

    [Header("AmmoControl")]
    public AmmoControl ammoControl;
    private bool shouldFire = false;
    

    [Header("AccurateReload")]
    public float reloadGracePeriod = 0.5f; // 0.5 seconds after landing to press reload
    private float timeSinceLanded = 0;
    private bool withinReloadWindow = false;


    void OnEnable()
    {
        PlayerController.OnGroundCheck += HandleLanding;
        PlayerController.IsAirborne += HandleMidair;
    }

    void OnDisable()
    {
        PlayerController.OnGroundCheck -= HandleLanding;
        PlayerController.IsAirborne -= HandleMidair;
    }

    private void HandleLanding()
    {
        if (ammo == 0)
        {
            ammo = 1;
            ammoControl.UpdateAmmos(ammo);
        }

        canReload = true;
        withinReloadWindow = true;
        timeSinceLanded = 0;

        
    }

    private void HandleMidair()
        {
            canReload = false;
        }


    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        ammo = maximumAmmo;
        gunPosition = transform.Find("BulletTransform");


        if (ammoControl != null)
        {
            ammoControl.SetAmmos(maximumAmmo);
            ammoControl.UpdateAmmos(ammo);
        }
    }

    void Update()
    {

        ProcessAccurateReload();

        if(ammo!=maximumAmmo)
        {
            magazineIsFull = false;
        }

        if (Input.GetMouseButtonDown(0) && ammo > 0 && !isReloading)
        {
            shouldFire = true;
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading&&canReload&&!magazineIsFull)
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

            if (ammoControl != null)
            {
                ammoControl.UpdateAmmos(ammo);
            }

    }
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        float reloadProgress = 0.0f;
        float rate = 1.0f / reloadTime;

        // Initialize fill amount to zero
        loadingCircle.fillAmount = 0;

        while (reloadProgress < 1.0f)
        {
            loadingCircle.fillAmount = reloadProgress;
            reloadProgress += rate * Time.deltaTime;
            yield return null;  // Wait a frame then continue
        }

        loadingCircle.fillAmount = 1.0f;  // Ensure it's fully filled

        ammo = maximumAmmo; // Reset ammo count
        magazineIsFull = true;
        isReloading = false;
        if (ammoControl != null)
        {
            ammoControl.UpdateAmmos(ammo);
        }
        Debug.Log("Reloaded");

        // Optionally, hide or reset the loading circle after reloading
        yield return new WaitForSeconds(0.5f); // Delay before hiding/resetting the fill
        loadingCircle.fillAmount = 0;
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


    private void ProcessAccurateReload()
    {
        if (withinReloadWindow && Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            ammo= maximumAmmo;
    
            ammoControl.UpdateAmmos(ammo);
            magazineIsFull = true;

            withinReloadWindow = false; // Close the window once reloaded
        }

        // Update timer if within the reload window
        if (withinReloadWindow)
        {
            timeSinceLanded += Time.deltaTime;
            if (timeSinceLanded > reloadGracePeriod)
            {
                withinReloadWindow = false; // Close the window if time expires
            }
        }
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

