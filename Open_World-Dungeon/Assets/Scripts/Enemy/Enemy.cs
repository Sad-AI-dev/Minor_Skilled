using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public GameObject dropItem;

    public int MaxHealth;
    public int CurrentHealth;

    public float Speed;

    public Slider HealthBar;

    public float AttackRange = 2;
    public float AttackSpeed = 2;
    public int Damage = 1;

    protected bool inRange;

    public virtual void Start()
    {
        if(agent) agent.speed = Speed;
        HealthBar.maxValue = MaxHealth;
    }

    public virtual void Update()
    {
        HealthBar.value = CurrentHealth;
        if(CurrentHealth <= 0)
        {
            if(dropItem) Instantiate(dropItem, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }
    
}
