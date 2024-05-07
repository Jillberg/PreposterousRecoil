using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUps : MonoBehaviour,IItem
{
    public static event Action CollectExtraHealth;

    public void Collect()
    {
        Destroy(gameObject);
        CollectExtraHealth?.Invoke();
    }
}
