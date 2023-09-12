using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public HealthManager health;
    [HideInInspector] public Interactor interactor;
    [HideInInspector] public PlayerAbilities abilities;
    [HideInInspector] public ItemInventory inventory;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<HealthManager>();
        interactor = GetComponent<Interactor>();
        abilities = GetComponent<PlayerAbilities>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            interactor.TryInteract();
        }
        if (Input.GetMouseButton(0)) {
            abilities.Shoot();
        }

        //========= Inventory ==========
        if (Input.GetKeyDown(KeyCode.Tab)) {
            Cursor.lockState = CursorLockMode.None;
            inventory.inventoryUI.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab)) {
            Cursor.lockState = CursorLockMode.Locked;
            inventory.inventoryUI.SetActive(false);
        }

        //TEMP
        if (Input.GetKeyDown(KeyCode.I)) { inventory.DebugSlotState(); }
    }
}
