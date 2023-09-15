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
    private PlayerHealth playerHealthManager;

    private int maxHealth = 100;
    private int health;

    private bool canTakeDmg = true;
    private bool canDealDmg = true;

    private float aggroRange = 25;

    private int damage = 10;

    float distanceToPlayer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealthManager = player.GetComponent<PlayerHealth>();

        agent = GetComponent<NavMeshAgent>();

        healthBarWidth = healthBar.transform.localScale.x;
        healthBarChunk = healthBarWidth / maxHealth;

        health = maxHealth;
    }

    private void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;

        if (distanceToPlayer < aggroRange)
        {
            agent.SetDestination(player.transform.position);
        }

        if(distanceToPlayer <= 2f)
        {
            if(canDealDmg)
            {
                StartCoroutine(DamagePlayer());
                //Debug.Log("In range");
            }
        }
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

    IEnumerator DamagePlayer()
    {
        canDealDmg = false;
        Debug.Log("Dealt Damage");
        playerHealthManager.TakeDamage(damage);
        yield return new WaitForSeconds(5f);
        canDealDmg = true;
    }
}
