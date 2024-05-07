using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPowerUps : MonoBehaviour, IItem
{
    public static event Action CollectExtraAmmo;

    public void Collect()
    {
        Destroy(gameObject);
        CollectExtraAmmo?.Invoke();
    }
}
