using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoControl : MonoBehaviour
{
    public Image CartridgePrefab;
    public Sprite AmmoLoaded;
    public Sprite AmmoUsed;

    private List<Image> ammos = new List<Image>();

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

