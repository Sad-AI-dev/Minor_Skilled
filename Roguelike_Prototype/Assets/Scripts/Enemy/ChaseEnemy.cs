using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChaseEnemy : MonoBehaviour
{
    private enum State { chase, hurt }

    public float destinationUpdateFrequency = 1f;

    [Header("Attack Settings")]
    public float damage;
    public float attackSpeed;
    [Space(10)]
    public float castOffset = 1f;
    public float castSize = 1f;
    public LayerMask attackMask;

    //external components
    private NavMeshAgent agent;
    private Player player;
    //vars
    private State state;
    private bool canAttack;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameManager.instance.player;
        canAttack = true;

        Act();
    }

    private void Act()
    {
        switch (state) {
            case State.chase:
                StartCoroutine(UpdateDestinationCo());
                break;
            case State.hurt:
                StartCoroutine(HurtPlayerCo());
                break;
        }
    }

    //============ Chase State =============
    private IEnumerator UpdateDestinationCo()
    {
        agent.SetDestination(player.transform.position);
        yield return new WaitForSeconds(destinationUpdateFrequency);
        Act();
    }

    //============= Hurt State ==============
    private IEnumerator HurtPlayerCo()
    {
        //try attack player
        if (canAttack) {
            Attack();
            StartCoroutine(AttackCooldownCo());
        }
        yield return null;
        Act();
    }

    private void Attack()
    {
        if (state == State.hurt) { //player is in trigger
            player.health.TakeDamage(damage);
        }
    }

    private IEnumerator AttackCooldownCo()
    {
        canAttack = false;
        yield return new WaitForSeconds(60f / attackSpeed);
        canAttack = true;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    //============ Update States ==================
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            state = State.hurt;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            state = State.chase;
        }
    }
}
