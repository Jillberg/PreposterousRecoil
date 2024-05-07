using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAmmoPowerUps : MonoBehaviour,IItem
{
    public static event Action CollectMagicAmmo;

    public void Collect()
    {
        Destroy(gameObject);
        CollectMagicAmmo?.Invoke();
    }
}
