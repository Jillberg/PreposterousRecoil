using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour,IItem
{
    public static event Action CollectHealthPotion;

    public void Collect()
    {
        Destroy(gameObject);
        CollectHealthPotion?.Invoke();
    }
}
