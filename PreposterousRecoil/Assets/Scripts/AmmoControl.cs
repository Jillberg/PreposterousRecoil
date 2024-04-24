using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoControl : MonoBehaviour
{
    public Image CartridgePrefab;
    [SerializeField] private Sprite AmmoLoaded;
    public Sprite AmmoUsed;
    [SerializeField] private GameObject Player;
    //[SerializeField] private GameObject RotatePoint;
    public static event Action OnAmmoSpriteChange;

    [Header("Air Fryer")]
    public Sprite airAmmoLoaded;

    [Header("Fire Blaster")]
    public Sprite fireAmmoLoaded;

    [Header("Elf Legacy")]
    public Sprite magicAmmoLoaded;




    

    private List<Image> ammos = new List<Image>();



    void OnEnable()
    {
        Player.GetComponent<PlayerController>().OnAmmoChange += HandleAmmoChange;
    }

    void OnDisable()
    {
       // Player.GetComponent<PlayerController>().OnAmmoChange -= HandleAmmoChange;
    }

    private void HandleAmmoChange(object sender, PlayerController.OnAmmoChangeEventArgs e)
    {
        if(e.ammoType == 1)
        {
 
            AmmoLoaded = airAmmoLoaded;
        }
        else if (e.ammoType == 2)
        {
            AmmoLoaded= fireAmmoLoaded;
        }
        else if (e.ammoType == 3)
        {
            AmmoLoaded= magicAmmoLoaded;
        }
        OnAmmoSpriteChange?.Invoke();
      //  SetAmmos(RotatePoint.GetComponent<Aiming>().maximumAmmo);
    }

    public void SetAmmos(int maxAmmo)
    {
        foreach (Image ammo in ammos)
        {
            Destroy(ammo.gameObject);
        }

        ammos.Clear();

        for (int i = 0; i < maxAmmo; i++)
        {
            Image newAmmo = Instantiate(CartridgePrefab, transform);
            newAmmo.sprite = AmmoLoaded;
            //newAmmo.color = Color.red;
            ammos.Add(newAmmo);
        }
    }

    public void UpdateAmmos(int currentAmmos)
    {
        for (int i = 0; i < ammos.Count; i++)
        {
            if (i < currentAmmos)
            {
                ammos[i].sprite = AmmoLoaded;
                //ammos[i].color = Color.red;
            }
            else
            {
                ammos[i].sprite = AmmoUsed;
                //ammos[i].color = Color.white;
            }


        }
    }
}

