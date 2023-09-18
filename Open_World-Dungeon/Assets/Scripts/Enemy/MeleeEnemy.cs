using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (target)
        {
            if (!inRange) agent.SetDestination(target.position);
            else
            {
                agent.SetDestination(transform.position);
                transform.LookAt(target);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !inRange)
        {
            inRange = true;
            StartCoroutine(DoDamage());
        }
    }

    IEnumerator DoDamage()
    {
        PlayerData.instance.TakeDamage(Damage);
        yield return new WaitForSeconds(AttackSpeed);
        Vector3 PlayerPos, EnemyPos;
        PlayerPos = new Vector3(PlayerData.instance.transform.position.x, 0, PlayerData.instance.transform.position.z);
        EnemyPos = new Vector3(transform.position.x, 0, transform.position.z);
        Debug.Log(Vector3.Distance(PlayerPos, EnemyPos));
        if (Vector3.Distance(PlayerPos, EnemyPos) <= AttackRange + 0.5)
        {
            StartCoroutine(DoDamage());
        }
        else inRange = false;
    }
}
