using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class MagicPunch : MonoBehaviour
{
    public int damage;

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("Punched Enemy");
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
