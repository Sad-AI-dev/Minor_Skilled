using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public HealthManager health;
    [HideInInspector] public Interactor interactor;
    [HideInInspector] public PlayerAbilities abilities;
    [HideInInspector] public ItemHandler itemHandler;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<HealthManager>();
        interactor = GetComponent<Interactor>();
        abilities = GetComponent<PlayerAbilities>();
        itemHandler = GetComponent<ItemHandler>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E)) {
            interactor.TryInteract();
        }
        if (Input.GetMouseButton(0)) {
            abilities.Shoot();
        }
    }
}
