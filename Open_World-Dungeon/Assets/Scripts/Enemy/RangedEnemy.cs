using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Shooting specific")]
    bool shooting = false;
    public GameObject bullet;
    public Transform Shootpoint;
    public float BulletSpeed;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if(inRange && !shooting)
        {
            shooting = true;
            StartCoroutine(Shoot());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = true;
            target = other.transform;
            Debug.Log("player entered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            inRange = false;
            Debug.Log("player exited");
        }
    }

    IEnumerator Shoot()
    {
        GameObject b = Instantiate(bullet, Shootpoint.position, Quaternion.identity);
        Rigidbody brb = b.GetComponent<Rigidbody>();
        EnemyBullet BB = b.GetComponent<EnemyBullet>();
        BB.Damage = Damage;
        b.transform.LookAt(target);
        brb.AddForce(b.transform.forward * BulletSpeed, ForceMode.Force);
        yield return new WaitForSeconds(AttackSpeed);
        if (inRange) StartCoroutine(Shoot());
        else { shooting = false; }
    }
}
