using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int[] seeds = new int[2];
    public int health = 10;
    public int maxHealth = 10;
    public int shield = 0;
    public int maxShield = 3;
    public int lastHealth;

    public event Action OnDeath;
    public event Action OnDamage;

    void Start()
    {
        seeds[0] = 5;
        seeds[1] = 10;
    }

    void Update()
    {
        if(health < lastHealth)
        {
            OnDamage.Invoke();
        }
        lastHealth = health;
        for(int i = 0; i < seeds.Length; i++)
        {
            seeds[i] = Mathf.Clamp(seeds[i], 0, 99);
        }

        if (health <= 0)
        {
            OnDeath.Invoke();
        }
    }
}
