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
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private CameraController camController;

        [Header("Audio")]
        [SerializeField] private AudioPlayer audioPlayer;

        private bool canPlayFootstep = true;
        private bool gamePaused = false;

        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            //hide inventory by default
            inventory.SetActive(false);
        }

        void Update()
        {
            pauseInput();

            gamePaused = pauseMenu.paused;

            if (gamePaused) return;
            WalkInput();
            JumpInput();
            AbilitiesInput();
            InteractInput();
            InventoryInput();
        }
        
        private void WalkInput()
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            playerController.SetMoveDirection(moveInput);

            if (canPlayFootstep && moveInput.magnitude >= 0.1f)
            {
                StartCoroutine(FootstepCo());
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
            if (Input.GetMouseButtonDown(1))
            {
                agent.abilities.secondary.TryUse();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                agent.abilities.special.TryUse();
            }
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                agent.abilities.utility.TryUse();
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
                camController.LockCamera();
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                inventory.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                camController.UnlockCamera();
            }
        }

        private void pauseInput()
        {
            if (Input.GetKeyUp(KeyCode.Escape) && !gamePaused)
            {
                gamePaused = true;
                pauseMenu.ActivateMenu();
            }
            else if(Input.GetKeyUp(KeyCode.Escape) && gamePaused)
            {
                gamePaused = false;
                pauseMenu.DeactivateMenu();
            }
        }

        private IEnumerator FootstepCo()
        {
            canPlayFootstep = false;
            yield return new WaitForSeconds(0.5f);
            audioPlayer.Play();
            canPlayFootstep = true;
        }        
    }
}
