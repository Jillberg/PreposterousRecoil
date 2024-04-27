using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHearts : MonoBehaviour
{

    public int maximumHealth;
    private int currentHealth;


    private SpriteRenderer spriteRenderer;

    public HeartUI heartUI;
    public static event Action OnPlayerDied;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maximumHealth;
        heartUI.SetHearts(currentHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    /*private  void OnTriggerEnter2D(Collier2D collision)
    {

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null) 
        {
            TakeDamage(enemy.damage);
        }
    }*/

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        heartUI.UpdateHearts(currentHealth);

        StartCoroutine(GettingHit());
        if(currentHealth <= 0) {
             OnPlayerDied?.Invoke();
        }
    }

    private IEnumerator GettingHit()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
    
}
