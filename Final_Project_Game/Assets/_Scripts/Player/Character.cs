using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public int maxVal;
    public int currVal;

    public Stat(int cur, int max)
    {
        maxVal = max;
        currVal = cur;
    }
    public void Subtract(int amount)
    {
        currVal -= amount;
        if (currVal < 0)
            currVal = 0;
    }
    public void Add(int amount)
    {
        currVal += amount;
        if (currVal > maxVal)
            currVal = maxVal;
    }
    public void SetToMax()
    {
        currVal = maxVal;
    }
}
public class Character : MonoBehaviour
{
    public Stat hp;
    public Stat stamina;
    private bool isDead;
    private bool isExhausted;
    [SerializeField] private StatusBar hpBar;
    [SerializeField] private StatusBar staminaBar;

    private void Start()
    {
        UpdateHPBar();
        UpdateStaminaBar();
    }

    public void TakeDamage(int amount)
    {
        hp.Subtract(amount);
        if (hp.currVal <= 0)
        {
            isDead = true;
        }
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        hpBar.SetSlideValue(hp.currVal, hp.maxVal);
    }  
    private void UpdateStaminaBar()
    {
        staminaBar.SetSlideValue(stamina.currVal, stamina.maxVal);
    }

    public void Heal(int amount)
    {
        hp.Add(amount);
        UpdateHPBar();
    }

    public void FullHealth()
    {
        hp.SetToMax();
        UpdateHPBar();
    }

    public void GetTired(int amount)
    {
        stamina.Subtract(amount);
        if (stamina.currVal < 0)
        {
            isExhausted = true;
        }
        UpdateStaminaBar();
    }

    public void Rest(int amount)
    {
        stamina.Add(amount);
        UpdateStaminaBar();

    }

    public void FullRest()
    {
        stamina.SetToMax();
        UpdateStaminaBar();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Heal(10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            FullHealth();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GetTired(10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Rest(10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            FullRest();
        }
    }
}
