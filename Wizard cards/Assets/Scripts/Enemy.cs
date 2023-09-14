using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RawImage healthBar;
    private float healthBarWidth;
    private float healthBarChunk;

    private NavMeshAgent agent;
    private GameObject player;

    private int maxHealth = 100;
    private int health;

    private bool canTakeDmg = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        agent = GetComponent<NavMeshAgent>();

        healthBarWidth = healthBar.transform.localScale.x;
        healthBarChunk = healthBarWidth / maxHealth;

        health = maxHealth;

        
    }

    private void Update()
    {
        agent.SetDestination(player.transform.position);
    }

    public void TakeDamage(int damageTaken)
    {
        if (!canTakeDmg) return;

        health -= damageTaken;

        StartCoroutine(TakeDamageCooldown());

        healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x - healthBarChunk * damageTaken, healthBar.transform.localScale.y, healthBar.transform.localScale.z);

        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject);
        }
    }

    IEnumerator TakeDamageCooldown()
    {
        canTakeDmg = false;
        yield return new WaitForSeconds(0.1f);
        canTakeDmg = true;
    }
}
