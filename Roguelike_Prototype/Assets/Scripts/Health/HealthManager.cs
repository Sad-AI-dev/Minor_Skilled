using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public float health;
    public float maxHealth;

    [Header("Settings")]
    public HealthBar healthBar;

    [Space(10)]
    [Tooltip("When true, triggers onHit event when onDeath is triggered")]
    public bool hitOnDeath = false;
    [Tooltip("When true, allows healing to surpass max health")]
    public bool allowOverHeal = false;
    [Tooltip("when true, allows healing through taking negative damage")]
    public bool allowNegDamage = false;
    [Tooltip("when true, allows taking damage through negative healing")]
    public bool allowNegHeal = false;

    [Header("Events")]
    public UnityEvent<float> onHit;
    public UnityEvent<float> onHeal;

    public UnityEvent onDeath;

    private void Start()
    {
        if (maxHealth <= 0f) { maxHealth = health; }
        if (healthBar != null) { UpdateHealthBar(); } //initialize health bar
    }

    //========= manage health =========
    public void TakeDamage(float damage)
    {
        if (!allowNegDamage && damage < 0f) { return; } //neg damage check
        //take damage
        health -= damage;
        HandleHealthChange(-damage);
        //health bar
        UpdateHealthBar();
    }

    public void Heal(float toHeal)
    {
        if (!allowNegHeal && toHeal < 0f) { return; } //neg heal check
        //heal
        health += toHeal;
        HandleHealthChange(toHeal);
        //health bar
        UpdateHealthBar();
    }

    private void HandleHealthChange(float healthChange)
    {
        //handle health
        bool died = health <= 0f;
        health = Mathf.Clamp(health, 0f, allowOverHeal? health : maxHealth); //if not allowOverHeal clamp to maxhealth, else clamp to healt (I.E. no limit)
        //call events
        if (died) { 
            if (hitOnDeath) { onHit?.Invoke(healthChange); }
            onDeath?.Invoke();
        }
        else {
            (healthChange > 0 ? onHeal : onHit)?.Invoke(healthChange);
        }
    }

    //============ manage health bar ============
    private void UpdateHealthBar()
    {
        if (healthBar != null) {
            healthBar.UpdateHealthBar(health / maxHealth);
        }
    }
}
