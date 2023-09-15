using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;

    [Header("UI")]
    [SerializeField] private GameObject HealthBar;
    [SerializeField] private Text text;
    private float healthBarLength;
    private float healthBarChunk;

    private void Start()
    {
        health = maxHealth;
        healthBarLength = HealthBar.transform.localScale.x;
        healthBarChunk = healthBarLength / maxHealth;
    }

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;
        HealthBar.transform.localScale = new Vector3(healthBarChunk * health, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        text.text = $"{health} / {maxHealth}";

        //Debug.Log("Damge taken: " + health);

        if(health <= 0)
        {
            //Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
