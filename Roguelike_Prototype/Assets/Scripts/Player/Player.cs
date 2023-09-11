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

        //TEMP
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            for (int i = 0; i < inventory.slots.Count; i++) {
                if (inventory.slots[i].State == ItemSlot.SlotState.filled) {
                    inventory.DropItem(inventory.slots[i].contents[0]);
                    break;
                }
            }
        }
    }
}
