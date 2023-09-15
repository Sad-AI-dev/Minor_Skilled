using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int Damage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerData.instance.TakeDamage(Damage);
        }

        if(other.tag != "Enemy") Destroy(gameObject);
    }
}
