using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerAmmoPowerUps : MonoBehaviour, IItem
{
    public static event Action CollectPowerAmmo;

    public void Collect()
    {
        Destroy(gameObject);
        CollectPowerAmmo?.Invoke();
    }
}
