using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public int MaxHealth;
    public int CurrentHealth;

    public TextMeshProUGUI healthText;

    private void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if(CurrentHealth <= 0)
        {
            Debug.Log("Game Over");
        }
        healthText.text = CurrentHealth.ToString() + " / " + MaxHealth.ToString();
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }
    public void Heal(int heal)
    {
        CurrentHealth += heal;
    }
}
