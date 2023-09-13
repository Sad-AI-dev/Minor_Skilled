using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    [HideInInspector] public int damage;

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Icicle hit Enemy");

            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
