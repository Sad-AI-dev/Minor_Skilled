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

            DamageMultiplier();
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    public void NullifyDamage()
    {
        damage = 0;
    }

    public virtual void DamageMultiplier()
    {

    }
}
