/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSprite : MonoBehaviour
{
    [SerializeField]private SpriteRenderer bulletSprite;

    [Header("Air Fryer")]
    public Sprite airAmmo;

    [Header("Fire Blaster")]
    public Sprite fireAmmo;

    [Header("Elf Legacy")]
    public Sprite magicAmmo;

    private GameObject Player;


    void Start()
    {
         Player = GameObject.FindGameObjectWithTag("Player");
    }
    void OnEnable()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        Player.GetComponent<PlayerController>().OnAmmoChange += HandleAmmoChange;

    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnDisable()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            Player.GetComponent<PlayerController>().OnAmmoChange -= HandleAmmoChange;
        }
    }

    private void HandleAmmoChange(object sender, PlayerController.OnAmmoChangeEventArgs e)
    {

        if (e.ammoType == 1)
        {

            bulletSprite.sprite = null;
            
            Debug.Log("bullet1");
        }
        else if (e.ammoType == 2)
        {
            bulletSprite.sprite = fireAmmo;
            Debug.Log("bullet2");
        }
        else if (e.ammoType == 3)
        {
            bulletSprite.sprite = magicAmmo;
            Debug.Log("bullet3");
        }
    }
}
*/