using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public int damage;
    public int enemyHealth;

    private SpriteRenderer spriteRenderer;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Damage(int damage)
    {
        enemyHealth-=damage;
        if(enemyHealth <= 0) {
            Destroy(gameObject);
        }
        StartCoroutine(GettingHit());
    }


    private IEnumerator GettingHit()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = Color.white;
    }
}
