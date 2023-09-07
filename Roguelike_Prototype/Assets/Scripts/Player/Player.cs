using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public HealthManager health;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<HealthManager>();
    }
}
