using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RawImage healthBar;
    private float healthBarWidth;
    private float healthBarChunk;

    private int maxHealth = 100;
    private int health;

    private void Start()
    {
        healthBarWidth = healthBar.transform.localScale.x;
        healthBarChunk = healthBarWidth / maxHealth;

        health = maxHealth;
    }

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;

        healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x - healthBarChunk * damageTaken, healthBar.transform.localScale.y, healthBar.transform.localScale.z);

        if (health < 0)
        {
            health = 0;
            Destroy(gameObject);
        }
    }
}
