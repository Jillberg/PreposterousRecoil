using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("SystemVariables")]
    public Rigidbody2D rb;
    Transform rotatePoint;
    public ParticleSystem dust;
    [SerializeField] private GameObject RotatePoint;
    public Animator animator;
    private bool canMove = true;
    public static event Action OnGroundCheck;
    public static event Action IsAirborne;
    public static event Action OnLandingStunBegin;
    public static event Action OnLandingStunEnd;
    public event EventHandler<OnAmmoChangeEventArgs> OnAmmoChange;
    public class OnAmmoChangeEventArgs : EventArgs
    {
        public int ammoType;
    }

    private bool beingHit = false;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;
    private bool isFacingRight = true;
    

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize=new Vector2(0.5f,0.5f);
    public LayerMask groundLayer;
    private bool wasGroundedLastFrame = false;

    [Header("Gravity")]
    public float baseGravity = 10f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    [Header("Falling")]
    public float stunThreshold = -30f;
    /*private float fallingSpeed;
    private Vector2 fallingPosition = new Vector2(0f, 0f);
    private float fallingTime;*/
    private float speedJustBeforeLanding;
    public float stunTime = 1f;


    void OnEnable()
    {
        RotatePoint.GetComponent<Aiming>().OnRecoilStart += HandleRecoilStart;
        RotatePoint.GetComponent<Aiming>().OnRecoilEnd += HandleRecoilEnd;
        PlayerHearts.OnHitBegin += HandleOnHitBegin;
        PlayerHearts.OnHitEnd += HandleOnHitEnd;
    }

    void OnDisable()
    {
        RotatePoint.GetComponent<Aiming>().OnRecoilStart -= HandleRecoilStart;
        RotatePoint.GetComponent<Aiming>().OnRecoilEnd -= HandleRecoilEnd;
        PlayerHearts.OnHitBegin -= HandleOnHitBegin;
        PlayerHearts.OnHitEnd -= HandleOnHitEnd;
    }


    private void HandleRecoilStart(object sender, Aiming.OnRecoilStartEventArgs e)
    {
        animator.SetBool(e.recoilAnimationCondition, true); 
         canMove = false;
    }

    private void HandleRecoilEnd(object sender, Aiming.OnRecoilEndEventArgs e)
    {
        animator.SetBool(e.recoilAnimationCondition, false);
        canMove = true;
    }

    private void HandleOnHitBegin()
    {
        beingHit=true;
    }
    private void HandleOnHitEnd()
    {
        beingHit = false;
    }


    void Start()
    {
        rotatePoint = transform.Find("RotatePoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (horizontalMovement == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
        
        if (canMove&&!beingHit)
        {
            
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
            
            // Flip();
        }
        ChangeAmmoType();
    }

    void FixedUpdate()
    {
        SendOnGroundCheckEvent();
        SendIsAirborneEvent();
        ProcessGravity();
        if (!IsGrounded())
        {
            speedJustBeforeLanding = rb.velocity.y;
        }
    }
    
    private void directionCheck()
    {
        if (transform.localScale.x > 0)
        {
            isFacingRight = true;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    private void Flip()
    {
        if (!isFacingRight && horizontalMovement > 0 || isFacingRight && horizontalMovement < 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            Vector3 cls = rotatePoint.localScale;
            ls.x *= -1;
           // cls.x *= -1;
           // cls.y *= -1;
            transform.localScale = ls;
         //   rotatePoint.localScale = cls;
         
        }
    
    }

    private bool IsGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            
            return true;
        }
       
        return false;
    }

    private void SendOnGroundCheckEvent()
    {
        bool currentStatus = IsGrounded();
       
        if (currentStatus&&! wasGroundedLastFrame)
        {
            OnGroundCheck?.Invoke();
            PlayEffect();
            if (speedJustBeforeLanding < stunThreshold)
            {
                Debug.Log("should stun");
                StartCoroutine(ProcessStun(stunTime));
            }
            
            
        }
        wasGroundedLastFrame= currentStatus;
    }
    
    IEnumerator ProcessStun(float stunTime)
    {
        OnLandingStunBegin?.Invoke();
        canMove = false;
        GetComponent<SpriteRenderer>().color = Color.blue;
        yield return new WaitForSeconds(stunTime);
        GetComponent<SpriteRenderer>().color = Color.white;
        canMove = true;
        OnLandingStunEnd?.Invoke();

    }


    private void SendIsAirborneEvent()
    {
        if (!IsGrounded())
        {
            IsAirborne?.Invoke();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);

    }

    private void ProcessGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;//make falling faster
            //rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void ChangeAmmoType()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Change ammo to air fryer");
            OnAmmoChange?.Invoke(this, new OnAmmoChangeEventArgs
            {
                ammoType=1
            });
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Change ammo to fire blaster");
            OnAmmoChange?.Invoke(this, new OnAmmoChangeEventArgs
            {
                ammoType = 2
            });
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Change ammo to elf legacy");
            OnAmmoChange?.Invoke(this, new OnAmmoChangeEventArgs
            {
                ammoType = 3
            });
        }
    }

    private void PlayEffect()
    {
        dust.Play();
    }
}
