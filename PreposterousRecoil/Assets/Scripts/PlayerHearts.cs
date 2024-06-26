using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHearts : MonoBehaviour
{

    public int maximumHealth;
    private int currentHealth;
    private bool canTakeDamage = true;
    public float knockBackForce;

    public static event Action OnHitBegin;
    public static event Action OnHitEnd;

    public Transform checkPoint;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;


    [Header("ParametersForBeingHit")]
    public float beingHitTime;
    public float knockBackTime;
    public HeartUI heartUI;
    public static event Action OnPlayerDied;
    public static event Action OnRespawn;
    private bool isInSwamp = false;
    private float swampTimer = 1.5f;
    private float poisonInterval = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maximumHealth;
        heartUI.SetHearts(currentHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        //checkPoint.position = new Vector3(0, 0, 0);

    }

    void OnEnable()
    {
        HealthPowerUps.CollectExtraHealth += IncreaseMaximumHealth;
        HealthPotion.CollectHealthPotion += HandleRestoreHealth;
    }

    void OnDisable()
    {
        HealthPowerUps.CollectExtraHealth -= IncreaseMaximumHealth;
        HealthPotion.CollectHealthPotion -= HandleRestoreHealth;
    }

    private void HandleRestoreHealth()
    {
        currentHealth = maximumHealth;
        heartUI.SetHearts(currentHealth);
    }

    public void Respawn()
    {
        currentHealth = maximumHealth;
        heartUI.SetHearts(currentHealth);
        transform.position=checkPoint.position;
        OnRespawn?.Invoke();
    }

    private void IncreaseMaximumHealth()
    {
        maximumHealth++;
        currentHealth = maximumHealth;
        heartUI.SetHearts(currentHealth);
        Debug.Log("++");
    }

     void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider=collision.collider;

        Enemy enemy = collider.GetComponent<Enemy>();
        
        if (enemy != null&&canTakeDamage) 
        {
            Vector2 fireDirection =(collider.GetComponent<Transform>().position- transform.position).normalized;
            rb.AddForce(fireDirection * knockBackForce, ForceMode2D.Impulse);
            float direction = collider.GetComponent<Transform>().position.x - transform.position.x;
            Debug.Log(fireDirection);
           
            TakeDamage(enemy.damage);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Trap trap = collision.GetComponent<Trap>();
        if (trap != null && canTakeDamage)
        {
            TakeDamage(trap.damage);
        }

        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null && canTakeDamage)
        {
            Vector2 fireDirection = (collision.GetComponent<Transform>().position - transform.position).normalized;
            rb.AddForce(fireDirection * knockBackForce, ForceMode2D.Impulse);
            float direction = collision.GetComponent<Transform>().position.x - transform.position.x;
            Debug.Log(fireDirection);

            TakeDamage(enemy.damage);
        }
        
        if (collision.transform.tag == "CheckPoint")
        {
            checkPoint= collision.transform;
        }

        if (collision.transform.tag == "PoisonousSwamp"&&!isInSwamp)
        {
            isInSwamp = true;
            if (isInSwamp)
            {
                Debug.Log("should start getting poisoned");
                StartCoroutine(GettingPoisoned());
            }

            Debug.Log("enter swamp");

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "PoisonousSwamp")
        {

            
            isInSwamp = false;
            Debug.Log("exit swamp");

        }
    }

    

    private IEnumerator GettingPoisoned()
    {
        while (isInSwamp)
        {
            Debug.Log("should be poisoned");
            while (swampTimer<poisonInterval)
            {
                swampTimer += Time.deltaTime;
                yield return null;  // Wait a frame then continue
                if(!isInSwamp )
                {
                    yield break;
                }
            }
            TakeDamage(1);
            swampTimer = 0;
            
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        heartUI.UpdateHearts(currentHealth);

        StartCoroutine(GettingHit());
        StartCoroutine(GettingKnockBack());
        if(currentHealth <= 0)
        {   
            OnPlayerDied?.Invoke();
       
        }
    }

    private IEnumerator GettingHit()
    {
        spriteRenderer.color = Color.red;
        canTakeDamage = false;
        
        yield return new WaitForSeconds(beingHitTime);
       
        spriteRenderer.color = Color.white;
        canTakeDamage=true;
    }

    private IEnumerator GettingKnockBack()
    {
        OnHitBegin?.Invoke();
        yield return new WaitForSeconds(knockBackTime);
        OnHitEnd?.Invoke();
    }

    }
