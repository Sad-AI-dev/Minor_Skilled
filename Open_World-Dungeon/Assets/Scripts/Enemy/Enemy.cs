using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;

    public int MaxHealth;
    public int CurrentHealth;

    public float Speed;

    public Slider HealthBar;

    private void Start()
    {
        agent.speed = Speed;
        HealthBar.maxValue = MaxHealth;
    }

    private void Update()
    {
        agent.SetDestination(target.position);

        HealthBar.value = CurrentHealth;
        if(CurrentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

}
