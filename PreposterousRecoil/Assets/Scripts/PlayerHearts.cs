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

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;


    [Header("ParametersForBeingHit")]
    public float beingHitTime;
    public float knockBackTime;
    public HeartUI heartUI;
    public static event Action OnPlayerDied;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maximumHealth;
        heartUI.SetHearts(currentHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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
