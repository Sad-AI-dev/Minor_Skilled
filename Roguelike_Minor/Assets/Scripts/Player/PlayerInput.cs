using Game.Core;
using Game.Core.GameSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player {
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInput : MonoBehaviour
    {
        private PlayerController playerController;
        [SerializeField] private Agent agent;
        [SerializeField] private Interactor interactor;
        [SerializeField] private GameObject inventory;
        [SerializeField] private AK.Wwise.Event Footsteps;
        
        private bool canPlayFootstep = true;

        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            //hide inventory by default
            inventory.SetActive(false);
        }

        void Update()
        {
            WalkInput();
            SprintInput();
            JumpInput();
            AbilitiesInput();
            InteractInput();
            InventoryInput();
        }
        
        private void WalkInput()
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            playerController.SetMoveDirection(moveInput);

            if (playerController.GetIsSprinting() && moveInput.y <= 0)
                playerController.Sprint(false);

            if (canPlayFootstep && moveInput.magnitude >= 0.1f)
            {
                StartCoroutine(FootstepCo());
            }
            
        }

        private void SprintInput()
        {
            if(Input.GetKeyDown(KeyCode.LeftShift) && !playerController.GetIsSprinting())
            {
                playerController.Sprint(true);
                return;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && playerController.GetIsSprinting())
            {
                playerController.Sprint(false);
            }
        }

        private void JumpInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerController.Jump();
            }
        }

        private void AbilitiesInput()
        {
            if(Input.GetMouseButton(0))
            {
                agent.abilities.primary.TryUse();
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                agent.abilities.special.TryUse();
            }
        }

        private void InteractInput()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactor.TryInteract();
            }
        }

        private void InventoryInput()
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                inventory.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                inventory.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private IEnumerator FootstepCo()
        {
            canPlayFootstep = false;
            yield return new WaitForSeconds(0.5f);
            Footsteps.Post(agent.gameObject);
            canPlayFootstep = true;
        }
        
    }
}
