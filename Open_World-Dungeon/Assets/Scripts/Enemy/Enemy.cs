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

    bool inRange;

    private void Start()
    {
        agent.speed = Speed;
        HealthBar.maxValue = MaxHealth;
    }

    private void Update()
    {
        if(!inRange) agent.SetDestination(target.position);
        else { 
            agent.SetDestination(transform.position);
            transform.LookAt(target);
        }

        HealthBar.value = CurrentHealth;
        if(CurrentHealth <= 0)
        {
            Instantiate(dropItem, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !inRange)
        {

            inRange = true;
            StartCoroutine(DoDamage());
            Debug.Log("DOing damage");
        }
    }

    IEnumerator DoDamage()
    {
        PlayerData.instance.TakeDamage(Damage);
        yield return new WaitForSeconds(AttackSpeed);
        Vector3 pp, ep;
        pp = new Vector3(PlayerData.instance.transform.position.x, 0, PlayerData.instance.transform.position.z);
        ep = new Vector3(transform.position.x, 0, transform.position.z);
        Debug.Log(Vector3.Distance(pp, ep));
        if (Vector3.Distance(pp, ep) <= AttackRange + 0.5)
        {
            StartCoroutine(DoDamage());
        }
        else inRange = false;
    }
}
