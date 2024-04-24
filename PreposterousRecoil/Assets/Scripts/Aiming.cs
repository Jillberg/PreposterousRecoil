using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Aiming;

public class Aiming : MonoBehaviour
{
    //should not mess around with these guys
    private Camera mainCam;
    private Vector3 mousePos;
    public Animator animator;
    private bool spriteChanged = false;
    [SerializeField] private GameObject Player;
    //public static event Action OnRecoilStart;
    //public static event Action OnRecoilEnd;
    public event EventHandler<OnRecoilStartEventArgs> OnRecoilStart;
    public class OnRecoilStartEventArgs : EventArgs
    {
        public string recoilAnimationCondition;
    }
    public event EventHandler<OnRecoilEndEventArgs> OnRecoilEnd;
    public class OnRecoilEndEventArgs : EventArgs
    {
        public string recoilAnimationCondition;
    }
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs:EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
      }

    private string recoilAnimationCondition;

    private Transform gunPosition;
    private Transform gunEndPointPosition;
    public Rigidbody2D rb;  // Reference to the Rigidbody2D component
    public Transform playerTransform;
    private float rotz;

    [Header("RecoilParameters")]
    public float recoilStrength = 5f;  // Adjustable recoil strength
    //private bool gunIsFacingRight = true;

    [Header("Ammunition")]
    private bool gunIsFacingRight = true;
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
        AmmoControl.OnAmmoSpriteChange += HandleAmmoSpriteChange;

        Player.GetComponent<PlayerController>().OnAmmoChange += HandleAmmoChange;
    }

    void OnDisable()
    {
        PlayerController.OnGroundCheck -= HandleLanding;
        PlayerController.IsAirborne -= HandleMidair;
        AmmoControl.OnAmmoSpriteChange -= HandleAmmoSpriteChange;
        Player.GetComponent<PlayerController>().OnAmmoChange -= HandleAmmoChange;
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

    private void HandleAmmoSpriteChange()
    {
        spriteChanged = true;
    }


    private void HandleAmmoChange(object sender, PlayerController.OnAmmoChangeEventArgs e)
    {
        StartCoroutine(UponChangingAmmo());
        
    }

    IEnumerator UponChangingAmmo()
    {
        yield return new WaitUntil(() => spriteChanged);
       // yield return new WaitForSeconds(1f);
        ammo = maximumAmmo; // Reset ammo count
        magazineIsFull = true;
        if (ammoControl != null)
        {
            ammoControl.SetAmmos(ammo);
        }
        Debug.Log("Reloaded");
        spriteChanged = false;
    }

    // Start is called before the first frame update
    void Start()
    {

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        ammo = maximumAmmo;
        gunPosition = transform.Find("BulletTransform");
        gunEndPointPosition = transform.Find("GunEndPointPosition");


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
       

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 rotation = mousePos - transform.position;
        rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotz);
        GunFlip();
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
    }

    private void Fire()
    {
            ammo--;
            animator.SetBool("isRecoiling", true);

      
        

        OnShoot?.Invoke(this,new OnShootEventArgs
            {
                gunEndPointPosition = gunEndPointPosition.position,
                shootPosition=mousePos,
        });
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
        Debug.Log(rotz);

    if (rotz > 45 && rotz < 135)
    {
        recoilAnimationCondition = "isShootingSkywards";
    }
    else if (rotz < -45 && rotz> -135)
    {
        recoilAnimationCondition = "isShootingDownwards";
        }
        else
    {
        // Not aiming upwards - trigger the normal recoil animation
        recoilAnimationCondition = "isPlayerRecoil";
    }

    OnRecoilStart?.Invoke(this, new OnRecoilStartEventArgs
    {
        recoilAnimationCondition = recoilAnimationCondition
    }) ;
    

    // Assuming the recoil effect lasts 0.5 seconds, adjust as needed
    StartCoroutine(EndRecoil(recoilAnimationCondition));
    }

    IEnumerator EndRecoil(string condition)
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isRecoiling",false);
        OnRecoilEnd?.Invoke(this, new OnRecoilEndEventArgs
        {
            recoilAnimationCondition = condition
        }) ;

    

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


     private void GunFlip()
      {
          if(mousePos.x < transform.position.x && gunIsFacingRight||mousePos.x>transform.position.x&&!gunIsFacingRight){
              gunIsFacingRight=!gunIsFacingRight;
              Vector3 ls = transform.localScale;
            Vector3 lls = playerTransform.localScale;

            ls.y *= -1;
            ls.x *= -1;
            lls.x *= -1;
             transform.localScale=ls;
              playerTransform.localScale=lls;

          }
      }


  

}

