using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int money = 1;

    //external components
    protected HealthManager health;
    protected Player player;

    private void Awake()
    {
        health = GetComponent<HealthManager>();
        health.onDeath.AddListener(OnDeath);
        player = GameManager.instance.player;
    }

    //============ Handle Death ==============
    private void OnDeath()
    {
        GameManager.instance.GainMoney(money);
    }
}
